using Facturas_simplified.Invoices.Entities;

namespace Facturas_simplified.Products.Entities;

public class Product
{
  public int Id { get; set; }
  public string? Name { get; set; }

  public required string Description { get; set; }
  public required decimal Amount { get; set; }


  // navigation props
  public ICollection<InvoiceDetail>? InvoiceDetails { get; set; }
}
