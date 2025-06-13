using Facturas_simplified.Abstractions;

namespace Facturas_simplified.Ncfs;

public class NcfRange : AuditableEntity
{
  public int Id { get; set; }
  public string? Description { get; set; }

  public required DateTime ValidFrom { get; set; }
  public required DateTime ValidTo { get; set; }


  public int NumberMin { get; set; }
  public int NumberMax { get; set; }
  public int CurrentNumber { get; set; }

  public required NcfRangeStatus NcfRangeStatus { get; set; }

  // navigation props
  public ICollection<Ncf>? Ncfs { get; set; }
}
