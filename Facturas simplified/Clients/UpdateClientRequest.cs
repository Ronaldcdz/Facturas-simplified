using System;
using Facturas_simplified.Database;
using Facturas_simplified.Clients;
using FluentValidation;

namespace Facturas_simplified.Clients;

public class UpdateClientRequest
{
  public required string Name { get; set; }
  public required string Rnc { get; set; }
  public string? Direction { get; set; }
  public required int ProvinceId { get; set; }
  public string? PhoneNumber { get; set; }
  public string? Email { get; set; }
  public string? Sector { get; set; }

}

public class UpdateClientRequestValidator : AbstractValidator<UpdateClientRequest>
{
  private readonly HttpContext _httpContext;
  private readonly AppDbContext _dbContext;
  public UpdateClientRequestValidator(IHttpContextAccessor httpContextAccessor, AppDbContext dbContext)
  {
    this._httpContext = httpContextAccessor.HttpContext!;
    _dbContext = dbContext;

    RuleFor(x => x.Direction).MustAsync(AddressNotBeEmptyIfItIsSetOnClientAlreadyAsync).WithMessage("La direccion no puede ser nula una vez ha sido establecida");
    // RuleFor(x => x.Sector).MustAsync(AddressNotBeEmptyIfItIsSetOnClientAlreadyAsync).WithMessage("El sector no puede ser nula una vez ha sido establecida");
    // RuleFor(x => x.ProvinceId).MustAsync(ProvinceNotBeEmptyIfItIsSetOnClientAlreadyAsync).WithMessage("La provincia no puede ser nula una vez ha sido establecida");
  }

  private async Task<bool> AddressNotBeEmptyIfItIsSetOnClientAlreadyAsync(string? address, CancellationToken token)
  {
    var id = Convert.ToInt32(_httpContext.Request.RouteValues["id"]);
    var employee = await _dbContext.Clients.FindAsync(id);

    if (employee!.Direction != null && string.IsNullOrWhiteSpace(address))
    {
      return false;
    }

    return true;
  }
  // private async Task<bool> ProvinceNotBeEmptyIfItIsSetOnClientAlreadyAsync(int? provinceId, CancellationToken token)
  // {
  //   var id = Convert.ToInt32(_httpContext.Request.RouteValues["id"]);
  //   var employee = await _dbContext.Clients.FindAsync(id);
  //
  //   if (employee!.ProvinceId != null && string.IsNullOrWhiteSpace(provinceId))
  //   {
  //     return false;
  //   }
  //
  //   return true;
  // }
}
