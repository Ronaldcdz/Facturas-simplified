namespace FacturasSimplified.Services.Dtos
{
  public class CreateServiceDto
  {
    public string? Name { get; set; }

    public required string Description { get; set; }
    public required double Amount { get; set; }
  }
}

