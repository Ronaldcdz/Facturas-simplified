namespace Facturas_simplified.Invoices.DTOs
{
  public record InvoiceCalculatedAmountsDto
  {
    public decimal Subtotal { get; init; }
    public decimal TaxAmount { get; init; }
    public decimal Total { get; init; }
  }

}

