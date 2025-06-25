using AutoMapper;
using Facturas_simplified.Clients.Dtos;
using Facturas_simplified.Provinces.Dtos;

namespace Facturas_simplified.Clients
{
  public class ClientMapping : Profile
  {

    public ClientMapping()
    {

      CreateMap<Client, ClientDto>();
    }

  }
}

