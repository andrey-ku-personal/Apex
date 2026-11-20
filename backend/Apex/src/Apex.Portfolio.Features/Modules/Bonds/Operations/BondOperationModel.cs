using Apex.Portfolio.Domain.Enums;
using Riok.Mapperly.Abstractions;
using BondOperationEntity = Apex.Portfolio.Domain.Entities.Bond.BondOperation;

namespace Apex.Portfolio.Features.Modules.Bonds.Operations;

public class BondOperationModel
{
    public int Id { get; set; }
    public int BondId { get; set; }
    public string UserId { get; set; } = null!;
    public OperationType Type { get; set; }
    public DateOnly Date { get; set; }
    public int Count { get; set; }
    public decimal Price { get; set; }
    public decimal Commission { get; set; }
}

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class BondOperationMapper
{
#pragma warning disable RMG020
#pragma warning disable RMG012

    public partial BondOperationModel MapToModel(BondOperationEntity entity);

#pragma warning restore RMG020
#pragma warning restore RMG012
}
