using AutoMapper;
using Facturas_simplified.Clients.Dtos;
using Facturas_simplified.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Facturas_simplified.Clients
{
  public class ClientController(ILogger<ClientController> logger, AppDbContext dbContext, IMapper mapper) : BaseController
  {
    private readonly ILogger<ClientController> _logger = logger;

    private readonly AppDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetClientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllClientsRequest request)
    {
      int page = request?.Page ?? 1;
      int numberOfRecords = request?.RecordsPerPage ?? 100;

      IQueryable<Client> query = _dbContext.Clients
          .Skip((page - 1) * numberOfRecords)
          .Take(numberOfRecords)
      .Include(c => c.Province);

      if (request != null)
      {
        if (!string.IsNullOrWhiteSpace(request.FirstNameContains))
        {
          query = query.Where(e => e.Name.Contains(request.FirstNameContains));
        }

        if (!string.IsNullOrWhiteSpace(request.LastNameContains))
        {
          query = query.Where(e => e.Rnc.Contains(request.LastNameContains));
        }
      }
      var clients = await query.ToArrayAsync();
      return Ok(_mapper.Map<ICollection<ClientDto>>(clients));

      // return Ok(clients.Select(client => new GetClientResponse
      // {
      //   Id = client.Id,
      //   Name = client.Name,
      //   Rnc = client.Rnc,
      //   Direction = client.Direction,
      //   ProvinceId = client.ProvinceId,
      //   PhoneNumber = client.PhoneNumber,
      //   Email = client.Email,
      //   Province = client.Province,
      //   Sector = client.Sector
      // }));
    }

    [HttpGet("{id}", Name = "GetClientById")]
    [ProducesResponseType(typeof(GetClientResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
      var client = await _dbContext.Clients.SingleOrDefaultAsync(client => client.Id == id);
      if (client == null)
      {
        return NotFound();
      }

      return Ok(new GetClientResponse
      {
        Id = client.Id,
        Name = client.Name,
        Rnc = client.Rnc,
        Direction = client.Direction,
        ProvinceId = client.ProvinceId,
        PhoneNumber = client.PhoneNumber,
        Email = client.Email,
        Province = client.Province
      });
    }


    [HttpPost]
    [ProducesResponseType(typeof(GetClientResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateClientRequest clientRequest)
    {
      // var validationResults = await ValidateAsync(clientRequest);
      // if (!validationResults.IsValid)
      // {
      //   return BadRequest(validationResults.ToModelStateDictionary());
      // }


      var newClient = new Client
      {
        Name = clientRequest.Name!,
        Rnc = clientRequest.Rnc!,
        Direction = clientRequest.Direction,
        ProvinceId = clientRequest.ProvinceId,
        PhoneNumber = clientRequest.PhoneNumber,
        Email = clientRequest.Email,
      };
      await _dbContext.Clients.AddAsync(newClient);
      await _dbContext.SaveChangesAsync();
      Console.WriteLine($"ID generado: {newClient.Id}");
      return CreatedAtRoute(
              "GetClientById",
              new { id = newClient.Id },
              new GetClientResponse
              {
                Id = newClient.Id,
                Name = newClient.Name,
                Rnc = newClient.Rnc,
                Direction = newClient.Direction,
                ProvinceId = newClient.ProvinceId,
                PhoneNumber = newClient.PhoneNumber,
                Email = newClient.Email,
                Province = newClient.Province
              }
          );
      // return CreatedAtAction(nameof(GetById), new { id = newClient.Id });
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(GetClientResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateClientRequest client)
    {
      _logger.LogInformation("Updating client with ID: {ClientId}", id);
      var existingClient = await _dbContext.Clients.FindAsync(id);
      if (existingClient == null)
      {
        _logger.LogWarning("Client with ID: {ClientId} not found", id);
        return NotFound();
      }


      // var validationResults = await ValidateAsync(client);
      // if (!validationResults.IsValid)
      // {
      //   return BadRequest(validationResults.ToModelStateDictionary());
      // }

      _logger.LogDebug("Updating client details for ID: {ClientId}", id);
      existingClient.Name = client.Name;
      existingClient.Rnc = client.Rnc;
      existingClient.Direction = client.Direction;
      existingClient.ProvinceId = client.ProvinceId;
      existingClient.PhoneNumber = client.PhoneNumber;
      existingClient.Email = client.Email;

      try
      {

        _logger.LogInformation("Client with ID: {ClientId} successfully updated", id);
        _dbContext.Entry(existingClient).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return Ok(existingClient);
      }
      catch (System.Exception ex)
      {
        _logger.LogError(ex, "Error occurred while updating client with ID: {ClientId}", id);
        return StatusCode(500, "An error occurred while updating the client");
      }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteClient(int id)
    {
      var client = await _dbContext.Clients.FindAsync(id);

      if (client == null)
      {
        return NotFound();
      }

      _dbContext.Clients.Remove(client);
      await _dbContext.SaveChangesAsync();

      return NoContent();
    }
  }


}
