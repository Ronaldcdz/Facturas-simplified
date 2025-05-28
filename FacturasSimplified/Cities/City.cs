using Facturas_simplified.Abstractions;

namespace Facturas_simplified.Cities;

public class City : AuditableEntity
{
  public int Id { get; set; }
  public required string Name { get; set; }
  public required int ProvinceId { get; set; }
  public string? Sector { get; set; }
}
