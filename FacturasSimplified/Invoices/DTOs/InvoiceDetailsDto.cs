using Facturas_simplified.Products.Dtos;

namespace Facturas_simplified.Invoices.DTOs
{
  public class InvoiceDetailDto
  {
    public int Id { get; set; }

    public int Quantity { get; set; }

    public required string Description { get; set; }
    public double UnitPrice { get; set; }
    public double SubTotal { get; set; }
    public string? SectionTitle { get; set; }
  }
}

