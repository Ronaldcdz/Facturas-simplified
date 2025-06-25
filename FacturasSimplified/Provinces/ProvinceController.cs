using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Facturas_simplified.Database;
using Microsoft.EntityFrameworkCore;
using Facturas_simplified.Provinces.Dtos;
using AutoMapper;

namespace Facturas_simplified.Provinces;

[Route("api/[controller]")]
[ApiController]
public class ProvinceController(ILogger<ProvinceController> logger, AppDbContext dbContext, IMapper mapper) : ControllerBase
{

  private readonly ILogger<ProvinceController> _logger = logger;

  private readonly AppDbContext _dbContext = dbContext;
  private readonly IMapper _mapper = mapper;

  [HttpGet]
  [ProducesResponseType(typeof(IEnumerable<GetProvinceResponse>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetAll()
  {
    var rawProvinces = await _dbContext.Provinces.ToListAsync();
    var provinces = _mapper.Map<ICollection<ProvinceDto>>(rawProvinces);

    return Ok(provinces);
  }
}
