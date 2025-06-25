using Facturas_simplified.Provinces.Dtos;

namespace Facturas_simplified.Clients.Dtos
{
  public class ClientDto
  {
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Rnc { get; set; }
    public string? Direction { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Sector { get; set; }

    // navigations properties
    public ProvinceDto? Province { get; set; }
  }
}

