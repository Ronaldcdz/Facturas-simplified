using System.Text.Json.Serialization;
using Facturas_simplified.Clients;

namespace Facturas_simplified.Provinces;

public class Province
{
    public int Id { get; set; }
    public required string Name { get; set; }

    [JsonIgnore]
    // Relaciones
    public ICollection<Client>? Clients { get; set; }
}
