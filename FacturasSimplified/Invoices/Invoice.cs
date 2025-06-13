using System;
using Facturas_simplified.Abstractions;
using Facturas_simplified.Clients;
using Facturas_simplified.Ncfs;
using Facturas_simplified.Payments;

namespace Facturas_simplified.Invoices;

public class Invoice : AuditableEntity
{
  public int Id { get; set; }

  public InvoiceStatus InvoiceStatus { get; set; } = InvoiceStatus.Issued;
  public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
  public DateTime? PaidAt { get; set; }
  public required InvoiceType InvoiceType { get; set; }

  public double Subtotal { get; set; }
  public double TaxPercentage { get; set; } = 0.18;
  public double TaxAmount { get; set; }
  public double Total { get; set; }

  public string? Note { get; set; }
  public string? AttentionTo { get; set; }
  public string? Project { get; set; }

  // navigations properties
  public int ClientId { get; set; }
  public Client? Client { get; set; }

  public int NcfId { get; set; }
  public Ncf? Ncf { get; set; }

  public ICollection<InvoiceDetail>? InvoiceDetails { get; set; }
  public ICollection<Payment>? Payments { get; set; }

}
