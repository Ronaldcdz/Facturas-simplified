using System;
using Facturas_simplified.Abstractions;
using Facturas_simplified.Clients.Entities;
using Facturas_simplified.Common.Enums;
using Facturas_simplified.Ncfs.Entities;
using Facturas_simplified.Payments.Entities;

namespace Facturas_simplified.Invoices.Entities;

public class Invoice : AuditableEntity
{
  public int Id { get; set; }

  public InvoiceStatus InvoiceStatus { get; set; } = InvoiceStatus.Issued;
  public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
  public DateTime? PaidAt { get; set; }
  public required InvoiceType InvoiceType { get; set; }

  public decimal Subtotal { get; set; }
  public decimal TaxPercentage { get; set; } = 0.18M;
  public decimal TaxAmount { get; set; }
  public decimal Total { get; set; }

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
