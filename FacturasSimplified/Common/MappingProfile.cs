using AutoMapper;
using Facturas_simplified.Clients.Dtos;
using Facturas_simplified.Clients.Entities;
using Facturas_simplified.Invoices.DTOs.Invoice;
using Facturas_simplified.Invoices.DTOs.InvoiceDetails;
using Facturas_simplified.Invoices.Entities;
using Facturas_simplified.Products.Dtos;
using Facturas_simplified.Products.Entities;
using Facturas_simplified.Provinces.Dtos;
using Facturas_simplified.Provinces.Entities;

namespace Facturas_simplified.Common
{
  public class MappingProfile : Profile
  {
    public MappingProfile(
        )
    {
      #region "Invoice"
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
        .ForMember(dest => dest.NcfNumber, opt => opt.MapFrom(src => src.Ncf.NcfNumber))
        .ForMember(dest => dest.NcfExpirationDate, opt => opt.MapFrom(src => src.Ncf.NcfRange.ValidTo));


      CreateMap<InvoiceDetail, InvoiceDetailDto>();


      CreateMap<UpdateInvoiceDto, Invoice>()
        .ForMember(dest => dest.InvoiceDetails, opt => opt.Ignore());

      CreateMap<UpdateInvoiceDto, CreateInvoiceDetailDto>();

      CreateMap<UpdateInvoiceDetailDto, CreateInvoiceDetailDto>();

      CreateMap<UpdateInvoiceDetailDto, InvoiceDetail>();

      CreateMap<UpdateInvoiceDetailDto, Product>()
        .ForMember(dest => dest.InvoiceDetails, opt => opt.Ignore());


      #endregion

      #region "Client"


      CreateMap<Client, ClientDto>();
      #endregion


      #region "Province"


      CreateMap<Province, ProvinceDto>();
      #endregion

    }
  }
}
