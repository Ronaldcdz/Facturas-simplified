using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Facturas_simplified.Clients;

public class CreateClientRequest
{
  [Required(AllowEmptyStrings = false)]
  public string? Name { get; set; }
  [Required(AllowEmptyStrings = false)]
  public string? Rnc { get; set; }
  public string? Direction { get; set; }
  public string? Sector { get; set; }
  public int ProvinceId { get; set; }
  public string? PhoneNumber { get; set; }
  public string? Email { get; set; }

}


public class CreateClientRequestValidator : AbstractValidator<CreateClientRequest>
{
  public CreateClientRequestValidator()
  {
    RuleFor(x => x.Name).NotEmpty().WithMessage("Nombre del cliente nulo");
    RuleFor(x => x.Rnc).NotEmpty().WithMessage("Rnc nulo");
  }
}
