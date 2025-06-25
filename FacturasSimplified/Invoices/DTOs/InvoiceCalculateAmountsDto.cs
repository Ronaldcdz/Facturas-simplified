namespace Facturas_simplified.Invoices.DTOs
{
  public record InvoiceCalculatedAmountsDto
  {
    public double Subtotal { get; init; }
    public double TaxAmount { get; init; }
    public double Total { get; init; }
  }

}

