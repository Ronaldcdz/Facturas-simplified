using AutoMapper;
using Facturas_simplified.Database;
using Facturas_simplified.Invoices.DTOs;
using Facturas_simplified.Ncfs;
using Facturas_simplified.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Facturas_simplified.Invoices
{
  [Route("api/[controller]")]
  [ApiController]
  public class InvoiceController(ILogger<InvoiceController> logger, AppDbContext dbContext, IMapper mapper) : BaseController
  {

    private readonly ILogger<InvoiceController> _logger = logger;
    private readonly AppDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;


    [HttpGet("{id}")]
    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
      var invoice = await _dbContext.Invoices
        .Include(i => i.Client)
        .Include(i => i.Ncf)
        .Include(i => i.InvoiceDetails)
        .FirstOrDefaultAsync(invoice => invoice.Id == id);

      if (invoice == null)
      {
        return NotFound("Factura no Encontrada");
      }

      return Ok(_mapper.Map<InvoiceDto>(invoice));
    }

    [HttpPost]
    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateInvoiceDto invoiceDto)
    {
      var validationResults = await ValidateAsync(invoiceDto);
      if (!validationResults.IsValid)
      {
        return BadRequest(validationResults.ToModelStateDictionary());
      }

      InvoiceService invoiceService = new(_dbContext, _mapper);
      var invoiceCreated = await invoiceService.CreateInvoiceAsync(invoiceDto);

      if (!invoiceCreated.IsSuccess)
      {
        return BadRequest(invoiceCreated.ErrorMessage);
      }


      return CreatedAtAction(nameof(GetById), new { id = invoiceCreated.Value.Id }, invoiceCreated.Value);
    }


  }
}
