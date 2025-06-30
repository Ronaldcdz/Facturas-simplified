using Facturas_simplified.Abstractions;
using Facturas_simplified.Products;

namespace Facturas_simplified.Invoices;

public class InvoiceDetail : AuditableEntity
{

  public int Id { get; set; }

  public required string Description { get; set; }

  public int Quantity { get; set; }
  public decimal UnitPrice { get; set; }
  public decimal SubTotal { get; set; }

  public string? SectionTitle { get; set; }

  // navigation props
  public int InvoiceId { get; set; }
  public Invoice? Invoice { get; set; }

  public int ProductId { get; set; }
  public Product? Product { get; set; }
}
