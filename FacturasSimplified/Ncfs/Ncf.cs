using Facturas_simplified.Abstractions;
using Facturas_simplified.Invoices;

namespace Facturas_simplified.Ncfs;

public class Ncf : AuditableEntity
{

  public int Id { get; set; }
  public InvoiceType Type { get; set; }

  public required string NcfNumber { get; set; }
  public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

  public required NcfStatus NcfStatus { get; set; } = NcfStatus.Used;

  // navigation props
  public int NcfRangeId { get; set; }
  public NcfRange? NcfRange { get; set; }

  public int InvoiceId { get; set; }
  public Invoice? Invoice { get; set; }
}
