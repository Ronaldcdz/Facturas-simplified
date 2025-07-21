using System.Text.Json.Serialization;
using Facturas_simplified.Clients.Entities;

namespace Facturas_simplified.Provinces.Entities;

public class Province
{
  public int Id { get; set; }
  public required string Name { get; set; }

  [JsonIgnore]
  // Relaciones
  public ICollection<Client>? Clients { get; set; }
}
