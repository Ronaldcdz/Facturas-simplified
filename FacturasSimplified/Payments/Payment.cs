using Facturas_simplified.Abstractions;
using Facturas_simplified.Invoices;

namespace Facturas_simplified.Payments;

public class Payment : AuditableEntity
{
  public int Id { get; set; }

  public DateTime PaidAt { get; set; }
  public required decimal Amount { get; set; }
  public required PaymentMethod PaymentMethod { get; set; } = PaymentMethod.BankTransfer;

  public string? Reference { get; set; }

  // navigation props
  public required int InvoiceId { get; set; }
  public Invoice? Invoice { get; set; }
}
