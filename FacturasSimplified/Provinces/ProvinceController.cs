using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Facturas_simplified.Database;
using Microsoft.EntityFrameworkCore;

namespace Facturas_simplified.Provinces;
[Route("api/[controller]")]
[ApiController]
public class ProvinceController : ControllerBase
{

    private readonly ILogger<ProvinceController> _logger;

    private readonly AppDbContext _dbContext;
    public ProvinceController(ILogger<ProvinceController> logger, AppDbContext dbContext)
    {
        _dbContext = dbContext;
        this._logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetProvinceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var provinces = await _dbContext.Provinces.ToListAsync();

        return Ok(provinces.Select(client => new GetProvinceResponse
        {
            Id = client.Id,
            Name = client.Name,
        }));
    }
}
