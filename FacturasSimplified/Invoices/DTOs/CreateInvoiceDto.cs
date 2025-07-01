using System.Text.Json.Serialization;
using Facturas_simplified.Database;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Facturas_simplified.Invoices.DTOs;

public class CreateInvoiceDto
{
  public InvoiceStatus InvoiceStatus { get; set; } = InvoiceStatus.Issued;
  public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

  public required InvoiceType InvoiceType { get; set; }

  public string? Note { get; set; }
  public string? AttentionTo { get; set; }
  public string? Project { get; set; }

  // navigations properties
  public int ClientId { get; set; }

  public required ICollection<CreateInvoiceDetailDto> InvoiceDetails { get; set; }


  [JsonIgnore]
  public decimal Subtotal { get; set; }
  [JsonIgnore]
  public decimal TaxPercentage { get; set; } = 0.18M;
  [JsonIgnore]
  public decimal TaxAmount { get; set; }
  [JsonIgnore]
  public decimal Total { get; set; }

}


public class CreateInvoiceDtoValidator : AbstractValidator<CreateInvoiceDto>
{
  private readonly AppDbContext _dbContext;
  public CreateInvoiceDtoValidator(AppDbContext dbContext)
  {
    _dbContext = dbContext;
    RuleFor(x => x.ClientId).MustAsync(ClientExists).WithMessage("El cliente no existe");
    RuleFor(x => x.InvoiceType).Must(IsInvoiceTypeValid).WithMessage("El tipo de factura no es valido");
    RuleFor(x => x.InvoiceDetails).NotNull().WithMessage("Debe seleccionar al menos un trabajo");
    RuleForEach(x => x.InvoiceDetails).SetValidator(new CreateInvoiceDetailDtoValidator());
  }


  private async Task<bool> ClientExists(int clientId, CancellationToken token)
  {
    return await _dbContext.Clients.AnyAsync(c => c.Id == clientId, cancellationToken: token);
  }
  private bool IsInvoiceTypeValid(InvoiceType type)
  {
    return Enum.IsDefined(typeof(InvoiceType), type);
  }
}




