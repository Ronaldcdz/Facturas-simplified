using AutoMapper;
using FacturasSimplified.Services.Dtos;

namespace Facturas_simplified.Services;

public class ServiceMapping : Profile
{
  public ServiceMapping()
  {
    CreateMap<CreateServiceDto, Service>()
      .ForMember(dest => dest.Id, opt => opt.Ignore())
      .ForMember(dest => dest.InvoiceDetails, opt => opt.Ignore());
  }
}
