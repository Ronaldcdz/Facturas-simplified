using AutoMapper;
using Facturas_simplified.Database;
using Facturas_simplified.Invoices;
using Facturas_simplified.Invoices.DTOs;
using Facturas_simplified.Utils;

namespace Facturas_simplified.Products
{
  public class ProductService(AppDbContext dbContext, IMapper mapper) : IProductService
  {
    private readonly AppDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<InvoiceCalculatedAmountsDto>> AddProductsAsync(List<CreateInvoiceDetailDto> createInvoiceDto, int invoiceId, decimal invoiceTax)
    {
      var products = MapAndCreateProducts([.. createInvoiceDto]);
      await _dbContext.Products.AddRangeAsync(products);
      await _dbContext.SaveChangesAsync();

      var details = BuildInvoiceDetails([.. createInvoiceDto], products, invoiceId);
      await _dbContext.InvoiceDetails.AddRangeAsync(details);
      await _dbContext.SaveChangesAsync();

      var totals = CalculateTotals(details, invoiceTax);
      return Result<InvoiceCalculatedAmountsDto>.Success(totals);
    }

    private List<Product> MapAndCreateProducts(List<CreateInvoiceDetailDto> dtos)
    {
      return _mapper.Map<List<Product>>(dtos);
    }

    private List<InvoiceDetail> BuildInvoiceDetails(List<CreateInvoiceDetailDto> dtos, List<Product> products, int invoiceId)
    {
      var invoiceDetailEntities = _mapper.Map<List<InvoiceDetail>>(dtos);

      for (int i = 0; i < invoiceDetailEntities.Count; i++)
      {
        invoiceDetailEntities[i].ProductId = products[i].Id;
        invoiceDetailEntities[i].InvoiceId = invoiceId;
        invoiceDetailEntities[i].SubTotal = dtos[i].UnitPrice * dtos[i].Quantity;
      }
      return invoiceDetailEntities;

    }

    private InvoiceCalculatedAmountsDto CalculateTotals(List<InvoiceDetail> dtos, decimal tax)
    {
      var invoiceDetailEntities = _mapper.Map<List<InvoiceDetail>>(dtos);
      decimal subTotal = 0;

      foreach (var detail in dtos)
      {
        subTotal += detail.SubTotal;
      }

      decimal taxAmount = subTotal * tax;
      return new InvoiceCalculatedAmountsDto
      {
        Subtotal = subTotal,
        TaxAmount = taxAmount,
        Total = subTotal + taxAmount
      };

    }


  }
}
