using Facturas_simplified.Abstractions;
using Facturas_simplified.Provinces;

namespace Facturas_simplified.Clients;

public class Client : AuditableEntity
{
  public int Id { get; set; }
  public required string Name { get; set; }
  public required string Rnc { get; set; }
  public string? Direction { get; set; }
  public string? PhoneNumber { get; set; }
  public string? Email { get; set; }
  public string? Sector { get; set; }

  // Relaciones
  // public ICollection<Factura>? Facturas { get; set; }    
  public int ProvinceId { get; set; }
  public Province? Province { get; set; }

}
