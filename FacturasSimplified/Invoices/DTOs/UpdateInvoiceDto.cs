
using Facturas_simplified.Database;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Facturas_simplified.Invoices.DTOs
{
  public class UpdateInvoiceDto
  {

    public int Id { get; set; }
    public InvoiceStatus InvoiceStatus { get; set; }
    public DateTime IssuedAt { get; set; }

    public required InvoiceType InvoiceType { get; set; }

    public string? Note { get; set; }
    public string? AttentionTo { get; set; }
    public string? Project { get; set; }

    // navigations properties
    public int ClientId { get; set; }

    public required ICollection<UpdateInvoiceDetailDto> InvoiceDetails { get; set; }


    // public decimal Subtotal { get; set; }
    // public decimal TaxPercentage { get; set; }
    // public decimal TaxAmount { get; set; }
    // public decimal Total { get; set; }

  }



  public class UpdateInvoiceDtoValidator : AbstractValidator<UpdateInvoiceDto>
  {
    private readonly AppDbContext _dbContext;
    public UpdateInvoiceDtoValidator(AppDbContext dbContext)
    {
      _dbContext = dbContext;
      RuleFor(x => x.ClientId).MustAsync(ClientExists).WithMessage("El cliente no existe");
      RuleFor(x => x.InvoiceType).MustAsync(ValidateInvoiceTypeChange).WithMessage("Error {ErrorCode}: {ErrorDescription}");
      RuleFor(x => x.InvoiceDetails).NotNull().WithMessage("Debe seleccionar al menos un trabajo");
      RuleForEach(x => x.InvoiceDetails).SetValidator(new UpdateInvoiceDetailDtoValidator());
    }


    private async Task<bool> ValidateInvoiceTypeChange(
            UpdateInvoiceDto dto,        // El objeto DTO completo (UpdateInvoiceDto)
            InvoiceType newInvoiceType,  // El valor de la propiedad InvoiceType del DTO
            ValidationContext<UpdateInvoiceDto> context, // El contexto de validación
            CancellationToken token)
    {
      var invoiceId = dto.Id;

      var invoice = await _dbContext.Invoices.FirstOrDefaultAsync(i => i.Id == invoiceId, cancellationToken: token);
      if (invoice is null)
      {
        context.MessageFormatter.AppendArgument("ErrorCode", "InvoiceNotFound");
        context.MessageFormatter.AppendArgument("ErrorDescription", "Factura no encontrada!!");
        return false;
      }

      if (invoice.InvoiceType != newInvoiceType)
      {
        context.MessageFormatter.AppendArgument("ErrorCode", "CannotChangeInvoiceType");
        context.MessageFormatter.AppendArgument("ErrorDescription", "El Tipo de Factura no se puede cambiar");
        return false;
      }

      return true;
    }

    private async Task<bool> ClientExists(int clientId, CancellationToken token)
    {
      return await _dbContext.Clients.AnyAsync(c => c.Id == clientId, cancellationToken: token);
    }
  }

}
