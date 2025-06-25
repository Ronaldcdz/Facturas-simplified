using Facturas_simplified.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Facturas_simplified.Ncfs
{
  [Route("api/[controller]")]
  [ApiController]
  public class NcfRangeController(ILogger<NcfRangeController> logger, AppDbContext dbContext) : BaseController
  {
    private readonly ILogger<NcfRangeController> _logger = logger;
    private readonly AppDbContext _dbContext = dbContext;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<NcfRange>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
      var ranges = await _dbContext.NcfRanges.ToListAsync();

      return Ok(ranges);
    }

  }
}
