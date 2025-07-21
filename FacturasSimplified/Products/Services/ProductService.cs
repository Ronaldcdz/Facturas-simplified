using AutoMapper;
using Facturas_simplified.Database;
using Facturas_simplified.Invoices.DTOs.Invoice;
using Facturas_simplified.Invoices.DTOs.InvoiceDetails;
using Facturas_simplified.Invoices.Entities;
using Facturas_simplified.Products.Entities;
using Facturas_simplified.Utils;

namespace Facturas_simplified.Products.Services
{
  public class ProductService(AppDbContext dbContext, IMapper mapper) : IProductService
  {
    private readonly AppDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<InvoiceCalculatedAmountsDto>> UpdateProductsAsync(List<UpdateInvoiceDetailDto> updateInvoiceDto, int invoiceId, decimal invoiceTax)
    {
      var products = _mapper.Map<List<Product>>(updateInvoiceDto);

      var productsToAdd = products.Where(p => p.Id == 0).ToList();
      await _dbContext.Products.AddRangeAsync(productsToAdd);
      await _dbContext.SaveChangesAsync();

      var deatilsToAdd = _mapper.Map<List<CreateInvoiceDetailDto>>(updateInvoiceDto.Where(i => i.ProductId == 0).ToList());
      var newDetails = BuildInvoiceDetails(deatilsToAdd, productsToAdd, invoiceId);
      await _dbContext.InvoiceDetails.AddRangeAsync(newDetails);
      await _dbContext.SaveChangesAsync();

      var productsAdded = products.Where(p => p.Id != 0).ToList();
      var currentDetailsMapped = _mapper.Map<List<CreateInvoiceDetailDto>>(updateInvoiceDto.Where(i => i.ProductId != 0).ToList());
      var currentDetails = BuildInvoiceDetails(currentDetailsMapped, productsAdded, invoiceId);


      var allDetails = newDetails.Concat(currentDetails).ToList();

      var totals = CalculateTotals(allDetails, invoiceTax);
      return Result<InvoiceCalculatedAmountsDto>.Success(totals);
    }
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
