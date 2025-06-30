using Facturas_simplified.Invoices.DTOs;
using Facturas_simplified.Utils;

namespace Facturas_simplified.Products
{
  public interface IProductService
  {
    Task<Result<InvoiceCalculatedAmountsDto>> AddProductsAsync(List<CreateInvoiceDetailDto> createInvoiceDto, int invoiceId, decimal invoiceTax);
  }
}
