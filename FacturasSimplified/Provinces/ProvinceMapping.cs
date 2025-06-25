using AutoMapper;
using Facturas_simplified.Provinces.Dtos;

namespace Facturas_simplified.Provinces
{
  public class ProvinceMapping : Profile
  {

    public ProvinceMapping()
    {

      CreateMap<Province, ProvinceDto>();
    }

  }
}

