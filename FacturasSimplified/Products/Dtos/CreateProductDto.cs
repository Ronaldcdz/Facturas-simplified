namespace Facturas_simplified.Products.Dtos
{
  public class CreateProductDto
  {
    public string? Name { get; set; }

    public required string Description { get; set; }
    public required decimal Amount { get; set; }
  }
}

