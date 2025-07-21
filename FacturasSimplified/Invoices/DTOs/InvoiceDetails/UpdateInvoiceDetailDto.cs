using System.Text.Json.Serialization;
using Facturas_simplified.Products.Entities;
using FluentValidation;

namespace Facturas_simplified.Invoices.DTOs.InvoiceDetails
{
  public class UpdateInvoiceDetailDto
  {

    public int Id { get; set; }
    public required string Description { get; set; }

    public required int Quantity { get; set; }
    public required decimal UnitPrice { get; set; }
    public decimal SubTotal { get; set; }

    public string? SectionTitle { get; set; }

    // navigation props
    public int? InvoiceId { get; set; }
    [JsonIgnore]
    public Entities.Invoice? Invoice { get; set; }

    public int? ProductId { get; set; }
    [JsonIgnore]
    public Product? Product { get; set; }
  }

  public class UpdateInvoiceDetailDtoValidator : AbstractValidator<UpdateInvoiceDetailDto>
  {
    public UpdateInvoiceDetailDtoValidator()
    {
      RuleFor(x => x.UnitPrice).GreaterThan(0).WithMessage("El precio no puede ser 0");
    }
  }
}
