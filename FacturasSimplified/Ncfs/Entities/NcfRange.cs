using Facturas_simplified.Abstractions;
using Facturas_simplified.Common.Enums;

namespace Facturas_simplified.Ncfs.Entities;

public class NcfRange : AuditableEntity
{
  public int Id { get; set; }
  public required InvoiceType Type { get; set; }
  public string? Description { get; set; }

  public required DateTime ValidFrom { get; set; }
  public required DateTime ValidTo { get; set; }


  public int NumberMin { get; set; }
  public int NumberMax { get; set; }
  public int CurrentNumber { get; set; }

  public NcfRangeStatus NcfRangeStatus { get; set; } = NcfRangeStatus.Active;

  // navigation props
  public ICollection<Ncf>? Ncfs { get; set; }
}
