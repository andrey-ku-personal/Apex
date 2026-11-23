using Apex.Portfolio.Features.Modules.Bonds.Details;
using Riok.Mapperly.Abstractions;
using BondEntity = Apex.Portfolio.Domain.Entities.Bond.Bond;

namespace Apex.Portfolio.Features.Modules.Bonds.Details;

public class BondModel
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public string Ticker { get; set; } = default!;
    public string Issuer { get; set; } = default!;

    public int CurrencyId { get; set; }
    public int PaymentFrequencyId { get; set; }

    public DateOnly IssueDate { get; set; }
    public DateOnly MaturityDate { get; set; }
    public int PaymentDay { get; set; }

    public decimal ParPrice { get; set; }
    public decimal Rate { get; set; }
    public bool IsRefinancingLinked { get; set; }
}

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class BondMapper
{
#pragma warning disable RMG020
#pragma warning disable RMG012

    public partial BondModel MapToModel(BondEntity entity);

#pragma warning restore RMG020
#pragma warning restore RMG012
}