using Facturas_simplified.Abstractions;
using Facturas_simplified.Services;

namespace Facturas_simplified.Invoices;

public class InvoiceDetail : AuditableEntity
{

  public int Id { get; set; }

  public required string Description { get; set; }

  public int Quantity { get; set; }
  public double UnitPrice { get; set; }
  public double SubTotal { get; set; }

  public string? SectionTitle { get; set; }

  // navigation props
  public int InvoiceId { get; set; }
  public Invoice? Invoice { get; set; }

  public int ServiceId { get; set; }
  public Service? Service { get; set; }
}
