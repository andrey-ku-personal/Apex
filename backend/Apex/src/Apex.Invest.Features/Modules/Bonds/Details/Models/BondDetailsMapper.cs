using Apex.Invest.Abstracts.Details;
using Apex.Invest.Domain.Entities.Bond;
using Apex.Invest.Features.Modules.Bonds.Details.Filters;
using Apex.Invest.Features.Modules.Bonds.Details.Models;
using Apex.Invest.Features.Modules.Bonds.Details.Queries;
using Apex.Shared.Core.Extensions;
using Riok.Mapperly.Abstractions;

namespace Apex.Invest.Features.Modules.Bonds.Details.Mapper;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class BondDetailsMapper : IBaseDetailsMapper<BondDetailsModel, Bond, BondDetailsFilter, BondDetailsQuery>
{
#pragma warning disable RMG020
#pragma warning disable RMG012
    public partial BondDetailsModel MapToModel(Bond entity);
    public partial BondDetailsFilter MapToFilter(Bond entity);
    public partial BondDetailsQuery MapToQuery(BondDetailsFilter filter);

    [MapProperty(nameof(BondOperationModel.Id), nameof(BondOperation.Id))]
    public partial BondOperation MapToBondOperation(BondOperationModel source);
    public partial void UpdateBondOperation(BondOperationModel source, BondOperation target);

    [MapperIgnoreTarget(nameof(Bond.Operations))]
    public partial void UpdateEntity(BondDetailsModel model, Bond entity);

    public Bond MapData(BondDetailsModel model, Bond entity)
    {
        UpdateEntity(model, entity);

        entity.Operations?.MergeCollections(
            model.Operations,
            entity => entity.Id,
            model => model.Id,
            MapToBondOperation,
            UpdateBondOperation
        );

        return entity;
    }

#pragma warning restore RMG020
#pragma warning restore RMG012
}
