using AutoMapper;
using Facturas_simplified.Products.Dtos;

namespace Facturas_simplified.Products;

public class ProductMapping : Profile
{
  public ProductMapping()
  {
    CreateMap<CreateProductDto, Product>()
      .ForMember(dest => dest.Id, opt => opt.Ignore())
      .ForMember(dest => dest.InvoiceDetails, opt => opt.Ignore());
  }
}
