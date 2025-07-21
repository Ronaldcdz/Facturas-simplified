using System;
using Facturas_simplified.Provinces.Entities;

namespace Facturas_simplified.Clients.Dtos;

public class GetClientResponse
{
  public int Id { get; set; }
  public required string Name { get; set; }
  public required string Rnc { get; set; }
  public string? Direction { get; set; }
  public string? PhoneNumber { get; set; }
  public string? Email { get; set; }
  public string? Sector { get; set; }

  public int? ProvinceId { get; set; }
  public required Province Province { get; set; }
}
