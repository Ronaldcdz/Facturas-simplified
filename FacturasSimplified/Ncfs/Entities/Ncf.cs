using Facturas_simplified.Abstractions;
using Facturas_simplified.Common.Enums;
using Facturas_simplified.Invoices.Entities;

namespace Facturas_simplified.Ncfs.Entities;

public class Ncf : AuditableEntity
{

  public int Id { get; set; }

  public required string NcfNumber { get; set; }
  public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

  public NcfStatus NcfStatus { get; set; } = NcfStatus.Used;

  // navigation props
  public required int NcfRangeId { get; set; }
  public NcfRange? NcfRange { get; set; }

  public int InvoiceId { get; set; }
  public Invoice? Invoice { get; set; }
}
