using AutoMapper;
using Facturas_simplified.Invoices.DTOs;
using Facturas_simplified.Services;
using FacturasSimplified.Services.Dtos;

namespace Facturas_simplified.Invoices;

public class InvoiceMapping : Profile
{
  public InvoiceMapping()
  {

    CreateMap<CreateInvoiceDto, Invoice>()
      .ForMember(dest => dest.InvoiceDetails, opt => opt.Ignore());

    CreateMap<CreateInvoiceDetailDto, CreateServiceDto>()
      .ForMember(dest => dest.Name, opt => opt.Ignore())
      .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.UnitPrice));


    CreateMap<CreateInvoiceDetailDto, InvoiceDetail>()
      .ForMember(dest => dest.Id, opt => opt.Ignore())
      .ForMember(dest => dest.Invoice, opt => opt.Ignore())
      .ForMember(dest => dest.Service, opt => opt.Ignore());

    CreateMap<CreateInvoiceDetailDto, Service>()
      .ForMember(dest => dest.Id, opt => opt.Ignore())
      .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.UnitPrice))
      .ForMember(dest => dest.InvoiceDetails, opt => opt.Ignore());

    CreateMap<Invoice, InvoiceDto>()
      .ForMember(dest => dest.NcfNumber, opt => opt.MapFrom(src => src.Ncf.NcfNumber));


    CreateMap<InvoiceDetail, InvoiceDetailDto>();
  }
}
