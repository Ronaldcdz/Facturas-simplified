using Facturas_simplified.Common.Enums;
using Facturas_simplified.Utils;

namespace Facturas_simplified.Ncfs.Services
{
  public interface INcfService
  {

    Task<Result<int>> AssignNextNcfAsync(InvoiceType invoiceType);
  }
}

