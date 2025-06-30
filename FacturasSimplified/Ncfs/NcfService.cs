using AutoMapper;
using Facturas_simplified.Database;
using Facturas_simplified.Invoices;
using Facturas_simplified.Utils;
using Microsoft.EntityFrameworkCore;

namespace Facturas_simplified.Ncfs
{

  public class NcfService(AppDbContext dbContext, IMapper mapper) : INcfService
  {
    private readonly AppDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<int>> AssignNextNcfAsync(InvoiceType invoiceType)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        var ncfRange = await _dbContext.NcfRanges
                                       .FirstOrDefaultAsync(n => n.Type == invoiceType);

        if (ncfRange is null)
        {
          return Result<int>.Failure("No existen rangos de NCFs creados para este tipo de factura.");
        }

        if (ncfRange.NcfRangeStatus != NcfRangeStatus.Active)
        {
          return Result<int>.Failure($"El rango de NCFs para este tipo de factura está en estado '{ncfRange.NcfRangeStatus.ToString()}' y no se puede usar.");
        }

        if (ncfRange.CurrentNumber >= ncfRange.NumberMax)
        {
          ncfRange.NcfRangeStatus = NcfRangeStatus.Exhausted;
          await _dbContext.SaveChangesAsync();
          return Result<int>.Failure("No quedan más NCFs para usar. El rango se ha agotado.");
        }

        ncfRange.CurrentNumber++;
        string nextNcfNumber = GenerateNcfNumber(invoiceType, ncfRange.CurrentNumber);

        var newNcfEntity = new Ncf { NcfNumber = nextNcfNumber, NcfRangeId = ncfRange.Id };
        await _dbContext.AddAsync(newNcfEntity);
        await _dbContext.SaveChangesAsync();

        await transaction.CommitAsync();

        return Result<int>.Success(newNcfEntity.Id);
      }
      catch (Exception ex)
      {
        await transaction.RollbackAsync();
        return Result<int>.Failure($"Ocurrió un error inesperado al asignar el NCF: {ex.Message}");
      }
    }

    private static string GenerateNcfNumber(InvoiceType type, int currentNumber, int sequenceLength = 8)
    {
      string prefix = type switch
      {
        InvoiceType.Fiscal => "B01",
        InvoiceType.Special => "B14",
        InvoiceType.Proforma => "P00",
        _ => throw new ArgumentOutOfRangeException(nameof(type), "Unsupported invoice type")
      };

      string paddedNumber = currentNumber.ToString().PadLeft(sequenceLength, '0');

      return prefix + paddedNumber;
    }
  }
}

