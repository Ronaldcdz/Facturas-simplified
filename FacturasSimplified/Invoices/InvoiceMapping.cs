using AutoMapper;
using Facturas_simplified.Invoices.DTOs;
using Facturas_simplified.Products;
using Facturas_simplified.Products.Dtos;

namespace Facturas_simplified.Invoices;

public class InvoiceMapping : Profile
{
  public InvoiceMapping()
  {

    CreateMap<CreateInvoiceDto, Invoice>()
      .ForMember(dest => dest.InvoiceDetails, opt => opt.Ignore());

    CreateMap<CreateInvoiceDetailDto, CreateProductDto>()
      .ForMember(dest => dest.Name, opt => opt.Ignore())
      .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.UnitPrice));


    CreateMap<CreateInvoiceDetailDto, InvoiceDetail>()
      .ForMember(dest => dest.Id, opt => opt.Ignore())
      .ForMember(dest => dest.Invoice, opt => opt.Ignore())
      .ForMember(dest => dest.Product, opt => opt.Ignore());

    CreateMap<CreateInvoiceDetailDto, Product>()
      .ForMember(dest => dest.Id, opt => opt.Ignore())
      .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.UnitPrice))
      .ForMember(dest => dest.InvoiceDetails, opt => opt.Ignore());

    CreateMap<Invoice, InvoiceDto>()
      .ForMember(dest => dest.NcfNumber, opt => opt.MapFrom(src => src.Ncf.NcfNumber));


    CreateMap<InvoiceDetail, InvoiceDetailDto>();


    CreateMap<UpdateInvoiceDto, Invoice>()
      .ForMember(dest => dest.InvoiceDetails, opt => opt.Ignore());

    CreateMap<UpdateInvoiceDto, CreateInvoiceDetailDto>();

    CreateMap<UpdateInvoiceDetailDto, CreateInvoiceDetailDto>();

    CreateMap<UpdateInvoiceDetailDto, InvoiceDetail>();

    CreateMap<UpdateInvoiceDetailDto, Product>()
      .ForMember(dest => dest.InvoiceDetails, opt => opt.Ignore());


  }
}
