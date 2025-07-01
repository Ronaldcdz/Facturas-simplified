using Facturas_simplified.Invoices.DTOs;
using Facturas_simplified.Utils;

namespace Facturas_simplified.Invoices
{
  public interface IInvoiceService
  {

    Task<Result<InvoiceDto>> UpdateInvoiceAsync(UpdateInvoiceDto updateInvoiceDto, int invoiceId);
    Task<Result<InvoiceDto>> AddInvoiceAsync(CreateInvoiceDto createInvoiceDto, int ncfId);
    Task<Result<InvoiceDto>> CreateInvoiceAsync(CreateInvoiceDto createInvoiceDto);

  }
}

