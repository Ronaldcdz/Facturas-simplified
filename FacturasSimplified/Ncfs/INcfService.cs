using Facturas_simplified.Invoices;
using Facturas_simplified.Utils;

namespace Facturas_simplified.Ncfs
{
  public interface INcfService
  {

    Task<Result<int>> AssignNextNcfAsync(InvoiceType invoiceType);
  }
}

