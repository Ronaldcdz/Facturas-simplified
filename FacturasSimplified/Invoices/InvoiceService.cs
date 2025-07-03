using AutoMapper;
using Facturas_simplified.Database;
using Facturas_simplified.Invoices.DTOs;
using Facturas_simplified.Ncfs;
using Facturas_simplified.Products;
using Facturas_simplified.Utils;
using Microsoft.EntityFrameworkCore;

namespace Facturas_simplified.Invoices
{
  public class InvoiceService(AppDbContext dbContext, IMapper mapper) : IInvoiceService
  {
    private readonly AppDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;


    public async Task<Result<InvoiceDto>> UpdateInvoiceAsync(UpdateInvoiceDto updateInvoiceDto, int invoiceId)
    {

      if (invoiceId != updateInvoiceDto.Id)
      {
        return Result<InvoiceDto>.Failure("El ID de la URL no coincide con el del cuerpo.");
      }

      var currentInvoice = await _dbContext.Invoices.Include(i => i.InvoiceDetails).ThenInclude(i => i.Product).FirstOrDefaultAsync(i => i.Id == invoiceId);
      if (currentInvoice is null)
      {
        return Result<InvoiceDto>.Failure("Esta Factura no existe");
      }

      using var transaction = await _dbContext.Database.BeginTransactionAsync();
      try
      {

        var updateResult = await SyncInvoiceDetailsAsync(updateInvoiceDto.InvoiceDetails.ToList(), currentInvoice);

        if (!updateResult.IsSuccess)
        {
          return Result<InvoiceDto>.Failure(updateResult.ErrorMessage);
        }

        _mapper.Map(updateInvoiceDto, currentInvoice); // solo actualiza propiedades necesarias

        currentInvoice.Subtotal = updateResult.Value.Subtotal;
        currentInvoice.TaxAmount = updateResult.Value.TaxAmount;
        currentInvoice.Total = updateResult.Value.Total;

        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        var invoiceDto = _mapper.Map<InvoiceDto>(currentInvoice);
        return Result<InvoiceDto>.Success(invoiceDto);
      }
      catch (System.Exception ex)
      {
        await transaction.RollbackAsync();
        return Result<InvoiceDto>.Failure($"Ocurrió un error inesperado al asignar el NCF: {ex.Message}");
      }
    }

    public async Task<Result<InvoiceDto>> CreateInvoiceAsync(CreateInvoiceDto createInvoiceDto)
    {

      NcfService ncfService = new(_dbContext, _mapper);
      var ncfResultId = await ncfService.AssignNextNcfAsync(createInvoiceDto.InvoiceType);
      if (!ncfResultId.IsSuccess)
      {
        return Result<InvoiceDto>.Failure(ncfResultId.ErrorMessage);
      }

      var invoice = await AddInvoiceAsync(createInvoiceDto, ncfResultId.Value);

      if (!invoice.IsSuccess)
      {
        // TODO: update to assigned method
        return Result<InvoiceDto>.Failure(invoice.ErrorMessage);
      }

      return Result<InvoiceDto>.Success(invoice.Value);
    }

    public async Task<Result<InvoiceDto>> AddInvoiceAsync(CreateInvoiceDto createInvoiceDto, int ncfId)
    {

      var invoiceMapped = _mapper.Map<Invoice>(createInvoiceDto);
      invoiceMapped.NcfId = ncfId;
      ProductService productService = new(_dbContext, _mapper);
      using var transaction = await _dbContext.Database.BeginTransactionAsync();
      try
      {

        await _dbContext.Invoices.AddAsync(invoiceMapped);
        await _dbContext.SaveChangesAsync();

        var result = await productService.AddProductsAsync(createInvoiceDto.InvoiceDetails.ToList(), invoiceMapped.Id, createInvoiceDto.TaxPercentage);
        if (!result.IsSuccess)
        {
          return Result<InvoiceDto>.Failure(result.ErrorMessage ?? "Hubo un problema agregando los servicios");
        }

        invoiceMapped.Total = result.Value.Total;
        invoiceMapped.Subtotal = result.Value.Subtotal;
        invoiceMapped.TaxAmount = result.Value.TaxAmount;
        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();
        return Result<InvoiceDto>.Success(_mapper.Map<InvoiceDto>(invoiceMapped));
      }
      catch (System.Exception ex)
      {
        await transaction.RollbackAsync();
        return Result<InvoiceDto>.Failure($"Ocurrió un error inesperado al asignar el NCF: {ex.Message}");
      }
    }


    private async Task<Result<InvoiceCalculatedAmountsDto>> SyncInvoiceDetailsAsync(
        List<UpdateInvoiceDetailDto> newDetails,
        Invoice currentInvoice)
    {
      var detailsToDelete = currentInvoice.InvoiceDetails.ExceptBy(
                 newDetails.Select(n => n.Id),
                 d => d.Id
             ).ToList();

      if (detailsToDelete.Count > 0)
      {
        _dbContext.InvoiceDetails.RemoveRange(detailsToDelete);
        _dbContext.Products.RemoveRange(detailsToDelete.Select(d => d.Product));
      }

      var detailsToUpdate = currentInvoice.InvoiceDetails.IntersectBy(
                 newDetails.Select(n => n.Id),
                 d => d.Id
             ).ToList();

      foreach (var detailInDb in detailsToUpdate)
      {
        var updatedData = newDetails.FirstOrDefault(d => d.Id == detailInDb.Id);

        if (updatedData != null)
        {
          detailInDb.Description = updatedData.Description;
          detailInDb.Quantity = updatedData.Quantity;
          detailInDb.UnitPrice = updatedData.UnitPrice;
          detailInDb.SectionTitle = updatedData.SectionTitle;
          detailInDb.SubTotal = updatedData.Quantity * updatedData.UnitPrice;
        }
      }



      var newInvoiceDetails = newDetails.Where(n => n.Id == 0).ToList();
      if (newInvoiceDetails.Count > 0)
      {
        // si hay InvoiceDetails con Id = 0 tambien lo seran los productos
        // porque no se permiten agregar desde un select sino que se agregan
        // on the fly
        var newProductsToAdd = newInvoiceDetails.Select(n => n.Product).ToList();
        await _dbContext.Products.AddRangeAsync(newProductsToAdd);
        await _dbContext.SaveChangesAsync();

        for (int i = 0; i < newInvoiceDetails.Count; i++)
        {
          newInvoiceDetails[i].ProductId = newProductsToAdd[i].Id;
          newInvoiceDetails[i].InvoiceId = currentInvoice.Id;
          newInvoiceDetails[i].SubTotal = newInvoiceDetails[i].UnitPrice * newInvoiceDetails[i].Quantity;
        }

        await _dbContext.InvoiceDetails.AddRangeAsync(_mapper.Map<List<InvoiceDetail>>(newInvoiceDetails));
      }

      var allDetails = detailsToUpdate.Concat(_mapper.Map<List<InvoiceDetail>>(newInvoiceDetails)).ToList();
      _dbContext.InvoiceDetails.UpdateRange(allDetails);

      var allSubtotal = allDetails.Select(a => a.SubTotal).Sum();
      var taxAmount = allSubtotal * currentInvoice.TaxPercentage;
      var total = allSubtotal + taxAmount;


      // await _dbContext.SaveChangesAsync();
      return Result<InvoiceCalculatedAmountsDto>.Success(new InvoiceCalculatedAmountsDto
      {
        Subtotal = allSubtotal,
        TaxAmount = taxAmount,
        Total = total
      });
    }
  }
}

