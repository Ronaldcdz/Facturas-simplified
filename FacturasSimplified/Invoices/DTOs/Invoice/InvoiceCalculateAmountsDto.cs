namespace Facturas_simplified.Invoices.DTOs.Invoice
{
  public record InvoiceCalculatedAmountsDto
  {
    public decimal Subtotal { get; init; }
    public decimal TaxAmount { get; init; }
    public decimal Total { get; init; }
  }

}

