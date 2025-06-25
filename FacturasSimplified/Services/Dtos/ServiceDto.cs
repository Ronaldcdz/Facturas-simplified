namespace Facturas_simplified.Services.Dtos
{
  public class ServiceDto
  {

    public int Id { get; set; }
    public string? Name { get; set; }

    public required string Description { get; set; }
    public required double Amount { get; set; }

  }
}

