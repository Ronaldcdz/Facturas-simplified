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
        return Result<InvoiceDto>.Failure("ncfResultId");
      }

      var invoice = await AddInvoiceAsync(createInvoiceDto, ncfResultId.Value);

      if (!invoice.IsSuccess)
      {
        return Result<InvoiceDto>.Failure(invoice.ErrorMessage);
      }

      return Result<InvoiceDto>.Success(invoice.Value);
    }

    public async Task<Result<InvoiceDto>> AddInvoiceAsync(CreateInvoiceDto createInvoiceDto, int ncfId)
    {
      var invoiceMapped = _mapper.Map<Invoice>(createInvoiceDto);

      if (invoiceMapped is null)
      {
        return Result<InvoiceDto>.Failure("No se pudo mapear");
      }
      invoiceMapped.NcfId = ncfId;
      await _dbContext.Invoices.AddAsync(invoiceMapped);
      await _dbContext.SaveChangesAsync();

      if (invoiceMapped.Id == 0)
      {
        return Result<InvoiceDto>.Failure("No se pudo guardar la entidad");
      }


      var result = await AddInvoiceDetailsAndCalculateAmountsAsync(createInvoiceDto.InvoiceDetails, invoiceMapped.Id, createInvoiceDto.TaxPercentage);
      if (!result.IsSuccess)
      {
        return Result<InvoiceDto>.Failure("Hubo un problema agregando con los servicios");
      }

      invoiceMapped.Total = result.Value.Total;
      invoiceMapped.Subtotal = result.Value.Subtotal;
      invoiceMapped.TaxAmount = result.Value.TaxAmount;
      await _dbContext.SaveChangesAsync();

      return Result<InvoiceDto>.Success(_mapper.Map<InvoiceDto>(invoiceMapped));
    }




    private async Task<Result<InvoiceCalculatedAmountsDto>> AddInvoiceDetailsAndCalculateAmountsAsync(
        ICollection<CreateInvoiceDetailDto> invoiceDetails, int invoiceId, double taxPercentage)
    {

      var services = new List<Product>();
      var invoiceDetailEntities = new List<InvoiceDetail>();

      double subtotal = 0;

      foreach (var detailDto in invoiceDetails)
      {
        var service = _mapper.Map<Product>(detailDto);
        services.Add(service);

        var invoiceDetail = _mapper.Map<InvoiceDetail>(detailDto);
        invoiceDetail.InvoiceId = invoiceId;
        invoiceDetail.SubTotal = detailDto.UnitPrice * detailDto.Quantity;
        subtotal += invoiceDetail.SubTotal;

        invoiceDetailEntities.Add(invoiceDetail);
      }

      await _dbContext.Products.AddRangeAsync(services);
      await _dbContext.SaveChangesAsync();

      for (int i = 0; i < invoiceDetailEntities.Count; i++)
      {
        invoiceDetailEntities[i].ProductId = services[i].Id;
      }

      await _dbContext.InvoiceDetails.AddRangeAsync(invoiceDetailEntities);
      await _dbContext.SaveChangesAsync();

      double taxAmount = subtotal * taxPercentage;
      double total = subtotal + taxAmount;

      return Result<InvoiceCalculatedAmountsDto>.Success(new InvoiceCalculatedAmountsDto
      {
        Subtotal = subtotal,
        TaxAmount = taxAmount,
        Total = total
      });
    }
  }
}

