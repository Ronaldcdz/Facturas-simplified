using Facturas_simplified.Invoices;

namespace Facturas_simplified.Products;

public class Product
{
  public int Id { get; set; }
  public string? Name { get; set; }

  public required string Description { get; set; }
  public required double Amount { get; set; }


  // navigation props
  public ICollection<InvoiceDetail>? InvoiceDetails { get; set; }
}
