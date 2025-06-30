using AutoMapper;
using Facturas_simplified.Database;
using Facturas_simplified.Invoices.DTOs;
using Facturas_simplified.Ncfs;
using Facturas_simplified.Products;
using Facturas_simplified.Utils;

namespace Facturas_simplified.Invoices
{
  public class InvoiceService(AppDbContext dbContext, IMapper mapper) : IInvoiceService
  {
    private readonly AppDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;


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
  }
}

