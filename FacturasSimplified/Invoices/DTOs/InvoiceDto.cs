using Facturas_simplified.Clients.Dtos;

namespace Facturas_simplified.Invoices.DTOs
{
  public class InvoiceDto
  {
    public int Id { get; set; }

    public InvoiceStatus InvoiceStatus { get; set; }
    public required DateTime IssuedAt { get; set; }
    public InvoiceType InvoiceType { get; set; }

    public double Subtotal { get; set; }
    public double TaxAmount { get; set; }
    public double Total { get; set; }

    public string? Note { get; set; }
    public string? AttentionTo { get; set; }
    public string? Project { get; set; }

    // navigations properties
    public ClientDto? Client { get; set; }

    public string? NcfNumber { get; set; }

    public ICollection<InvoiceDetailDto>? InvoiceDetails { get; set; }
  }
}

