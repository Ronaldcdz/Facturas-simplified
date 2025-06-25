using AutoMapper;
using Facturas_simplified.Database;
using Facturas_simplified.Invoices.DTOs;
using Facturas_simplified.Ncfs;
using Facturas_simplified.Services;
using Facturas_simplified.Utils;
using FacturasSimplified.Services.Dtos;
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


      var ncfRange = await _dbContext.NcfRanges.FirstOrDefaultAsync(n => n.NcfRangeStatus == NcfRangeStatus.Active && n.Type == invoiceDto.InvoiceType);
      if (ncfRange is null)
      {
        return BadRequest("No existen Ncfs validos para este tipo de factura");
      }

      if (ncfRange.CurrentNumber >= ncfRange.NumberMax)
      {
        //usar logica para marcar como agotado
        return BadRequest("No quedan mas NCF para usar");
      }


      ncfRange.CurrentNumber++;
      // _dbContext.Entry(ncfRange).State = EntityState.Modified;
      await _dbContext.SaveChangesAsync();

      //agregar capa de servicio que se encargue de verificar si no esta vencido o agregar validaciones para saber si el numero es valido

      var ncf = await _dbContext.AddAsync(new Ncf { NcfNumber = GenerateNcfNumber(invoiceDto.InvoiceType, ncfRange.CurrentNumber), NcfRangeId = ncfRange.Id });
      await _dbContext.SaveChangesAsync();


      var invoiceMapped = _mapper.Map<Invoice>(invoiceDto);
      invoiceMapped.NcfId = ncf.Entity.Id;
      var newInvoiceWithId = await _dbContext.Invoices.AddAsync(_mapper.Map<Invoice>(invoiceMapped));
      await _dbContext.SaveChangesAsync();



      var result = await AddServicesToBd(invoiceDto.InvoiceDetails, newInvoiceWithId.Entity.Id, invoiceDto.TaxPercentage);
      if (!result.IsSuccess)
      {
        return BadRequest("Hubo un problema agregando con los servicios");
      }

      newInvoiceWithId.Entity.Total = result.Value.Total;
      newInvoiceWithId.Entity.Subtotal = result.Value.Subtotal;
      newInvoiceWithId.Entity.TaxAmount = result.Value.TaxAmount;
      await _dbContext.SaveChangesAsync();

      return CreatedAtAction(nameof(GetById), new { id = newInvoiceWithId.Entity.Id }, invoiceDto);
    }

    private async Task<Result<InvoiceCalculatedAmountsDto>> AddServicesToBd(ICollection<CreateInvoiceDetailDto> invoiceDetails, int invoiceId, double taxPercentage)
    {
      try
      {

        double subTotal = 0;
        foreach (var detail in invoiceDetails)
        {
          var serviceId = await _dbContext.Services.AddAsync(_mapper.Map<Service>(detail));
          await _dbContext.SaveChangesAsync();

          InvoiceDetail invoiceDetail = _mapper.Map<InvoiceDetail>(detail);
          invoiceDetail.InvoiceId = invoiceId;
          invoiceDetail.ServiceId = serviceId.Entity.Id;
          invoiceDetail.SubTotal = detail.UnitPrice * detail.Quantity;
          subTotal += invoiceDetail.SubTotal;

          await _dbContext.InvoiceDetails.AddAsync(invoiceDetail);
          await _dbContext.SaveChangesAsync();

        }

        double taxAmount = subTotal * taxPercentage;
        double total = taxAmount + subTotal;
        InvoiceCalculatedAmountsDto invoiceData = new()
        {
          Subtotal = subTotal,
          TaxAmount = taxAmount,
          Total = total

        };
        return Result<InvoiceCalculatedAmountsDto>.Success(invoiceData);
      }
      catch (System.Exception)
      {
        return Result<InvoiceCalculatedAmountsDto>.Failure("Algo paso por aqui");
        throw;
      }
    }
    private static string GenerateNcfNumber(InvoiceType type, int currentNumber, int sequenceLength = 8)
    {
      string prefix = type switch
      {
        InvoiceType.Fiscal => "B01",
        InvoiceType.Special => "B14",
        InvoiceType.Proforma => "P00", // o cualquier otro
        _ => throw new ArgumentOutOfRangeException(nameof(type), "Unsupported invoice type")
      };

      string paddedNumber = currentNumber.ToString().PadLeft(sequenceLength, '0');

      return prefix + paddedNumber;
    }
  }
}
