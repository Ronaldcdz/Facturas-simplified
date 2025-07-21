using Facturas_simplified.Invoices.DTOs.Invoice;
using Facturas_simplified.Invoices.DTOs.InvoiceDetails;
using Facturas_simplified.Utils;

namespace Facturas_simplified.Products.Services
{
  public interface IProductService
  {
    Task<Result<InvoiceCalculatedAmountsDto>> AddProductsAsync(List<CreateInvoiceDetailDto> createInvoiceDto, int invoiceId, decimal invoiceTax);
    Task<Result<InvoiceCalculatedAmountsDto>> UpdateProductsAsync(List<UpdateInvoiceDetailDto> updateInvoiceDto, int invoiceId, decimal invoiceTax);
  }
}
