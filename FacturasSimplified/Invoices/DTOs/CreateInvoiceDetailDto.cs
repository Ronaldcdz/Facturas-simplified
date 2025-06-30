using System.Text.Json.Serialization;
using Facturas_simplified.Database;
using Facturas_simplified.Products;
using FluentValidation;

namespace Facturas_simplified.Invoices.DTOs;

public class CreateInvoiceDetailDto()
{

  public required string Description { get; set; }

  public required int Quantity { get; set; }
  public required double UnitPrice { get; set; }
  public double SubTotal { get; set; }

  public string? SectionTitle { get; set; }

  // navigation props
  public int? InvoiceId { get; set; }
  [JsonIgnore]
  public Invoice? Invoice { get; set; }

  public int? ProductId { get; set; }
  [JsonIgnore]
  public Product? Product { get; set; }
}



public class CreateInvoiceDetailDtoValidator : AbstractValidator<CreateInvoiceDetailDto>
{
  public CreateInvoiceDetailDtoValidator()
  {
    RuleFor(x => x.UnitPrice).GreaterThan(0).WithMessage("El precio no puede ser 0");
  }


}




