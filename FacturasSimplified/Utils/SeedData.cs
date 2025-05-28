using System;
using Facturas_simplified.Cities;
using Facturas_simplified.Database;
using Facturas_simplified.Clients;
using Facturas_simplified.Provinces;

namespace Facturas_simplified.Utils;

public static class SeedData
{
  public static void Seed(IServiceProvider serviceProvider)
  {
    var context = serviceProvider.GetRequiredService<AppDbContext>();
    // context.Database.EnsureCreated();

    #region Clientes
    if (!context.Clients.Any())
    {
      context.Clients.AddRange(
          new Client
          {
            Name = "Soluciones Innovadoras SRL",
            Rnc = "123456789",
            Direction = "Av. Independencia #45, Santo Domingo Este",
            ProvinceId = 2,
            PhoneNumber = "8095550101",
            Email = "contacto@solucionesinnovadoras.com"
          },
          new Client
          {
            Name = "Tecnología Avanzada RD",
            Rnc = "123456790",
            Direction = "Calle Duarte #78, Santiago de los Caballeros",
            ProvinceId = 3,
            PhoneNumber = "8295550202",
            Email = "ventas@tecnologiaavanzadard.com"
          },
          new Client
          {
            Name = "Construcciones Caribeñas CxA",
            Rnc = "123456791",
            Direction = "Av. Sarasota #12, La Vega",
            ProvinceId = 4,
            PhoneNumber = "8495550303",
            Email = "info@construccionescaribenhas.com"
          },
          new Client
          {
            Name = "Farmacia Salud Total",
            Rnc = "123456792",
            Direction = "Calle 27 de Febrero #33, San Pedro de Macorís",
            ProvinceId = 5,
            PhoneNumber = "8095550404",
            Email = "farmaciasaludtotal@gmail.com"
          },
          new Client
          {
            Name = "Distribuciones Nacionales",
            Rnc = "123456793",
            Direction = "Av. México #89, San Cristóbal",
            ProvinceId = 6,
            PhoneNumber = "8295550505",
            Email = "dnacionales@distribuciones.com"
          },
          new Client
          {
            Name = "Importaciones y Más",
            Rnc = "123456794",
            Direction = "Calle Sánchez #56, Higüey",
            ProvinceId = 7,
            PhoneNumber = "8495550606",
            Email = "importacionesymas@negocios.com"
          },
          new Client
          {
            Name = "ElectroHogar Dominicano",
            Rnc = "123456795",
            Direction = "Av. Winston Churchill #100, Santo Domingo",
            ProvinceId = 8,
            PhoneNumber = "8095550707",
            Email = "electrohogar@ehdominicano.com"
          },
          new Client
          {
            Name = "Agroindustria del Valle",
            Rnc = "123456796",
            Direction = "Carretera Mella Km 8, San Juan de la Maguana",
            ProvinceId = 9,
            PhoneNumber = "8295550808",
            Email = "agroindustria@valledelSur.com"
          },
          new Client
          {
            Name = "Moda Urbana SAS",
            Rnc = "123456797",
            Direction = "Plaza Central Local #24, Punta Cana",
            ProvinceId = 10,
            PhoneNumber = "8495550909",
            Email = "modaurbana@outlook.com"
          },
          new Client
          {
            Name = "Transporte Rápido CxA",
            Rnc = "123456798",
            Direction = "Autopista Las Américas Km 12, Boca Chica",
            ProvinceId = 11,
            PhoneNumber = "8095551010",
            Email = "transporterapido@logistica.com"
          }
      );

    }
    #endregion
    #region City

    // if (!context.Cities.Any())
    // {
    //
    //   context.Cities.AddRange(
    //       new City
    //       {
    //         Id = 1,
    //         Name = "Santo Domingo Este",
    //         ProvinceId = 1
    //       }
    //       );
    // }
    #endregion
    #region Province
    if (!context.Provinces.Any())
    {

      context.Provinces.AddRange(
        new Province { Id = 1, Name = "Distrito Nacional" },
        new Province { Id = 2, Name = "Azua" },
        new Province { Id = 3, Name = "Bahoruco" },
        new Province { Id = 4, Name = "Barahona" },
        new Province { Id = 5, Name = "Dajabón" },
        new Province { Id = 6, Name = "Duarte" },
        new Province { Id = 7, Name = "Elías Piña" },
        new Province { Id = 8, Name = "El Seibo" },
        new Province { Id = 9, Name = "Espaillat" },
        new Province { Id = 10, Name = "Hato Mayor" },
        new Province { Id = 11, Name = "Hermanas Mirabal" },
        new Province { Id = 12, Name = "Independencia" },
        new Province { Id = 13, Name = "La Altagracia" },
        new Province { Id = 14, Name = "La Romana" },
        new Province { Id = 15, Name = "La Vega" },
        new Province { Id = 16, Name = "María Trinidad Sánchez" },
        new Province { Id = 17, Name = "Monseñor Nouel" },
        new Province { Id = 18, Name = "Monte Cristi" },
        new Province { Id = 19, Name = "Monte Plata" },
        new Province { Id = 20, Name = "Pedernales" },
        new Province { Id = 21, Name = "Peravia" },
        new Province { Id = 22, Name = "Puerto Plata" },
        new Province { Id = 23, Name = "Samaná" },
        new Province { Id = 24, Name = "San Cristóbal" },
        new Province { Id = 25, Name = "San José de Ocoa" },
        new Province { Id = 26, Name = "San Juan" },
        new Province { Id = 27, Name = "San Pedro de Macorís" },
        new Province { Id = 28, Name = "Sánchez Ramírez" },
        new Province { Id = 29, Name = "Santiago" },
        new Province { Id = 30, Name = "Santiago Rodríguez" },
        new Province { Id = 31, Name = "Santo Domingo" },
        new Province { Id = 32, Name = "Valverde" }
      );
    }
    #endregion
    context.SaveChanges();
  }
}
