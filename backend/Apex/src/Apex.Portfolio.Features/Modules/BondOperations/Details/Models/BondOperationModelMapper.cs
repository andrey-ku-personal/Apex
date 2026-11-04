using Apex.Portfolio.Abstracts.Details;
using Apex.Portfolio.Features.Modules.BondOperations.Details.Filtering;
using Riok.Mapperly.Abstractions;
using BondOperationEntity = Apex.Portfolio.Domain.Entities.Bond.BondOperation;

namespace Apex.Portfolio.Features.Modules.BondOperations.Details.Models;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class BondOperationModelMapper : IBaseDetailsMapper<BondOperationModel, BondOperationEntity, BondOperationFilter, BondOperationQuery>
{
#pragma warning disable RMG020
#pragma warning disable RMG012

    public partial BondOperationModel MapToModel(BondOperationEntity entity);
    public partial BondOperationFilter MapToFilter(BondOperationEntity entity);
    public partial BondOperationQuery MapToQuery(BondOperationFilter filter);

    public BondOperationEntity MapToEntity(BondOperationModel model) => new()
    {
        BondId = model.BondId,
        UserId = model.UserId,
        Type = model.Type,
        Date = model.Date,
        Count = model.Count,
        Price = model.Price,
        Commission = model.Commission
    };

    public BondOperationEntity MapData(BondOperationModel model, BondOperationEntity entity)
    {
        entity.BondId = model.BondId;
        entity.UserId = model.UserId;
        entity.Type = model.Type;
        entity.Date = model.Date;
        entity.Count = model.Count;
        entity.Price = model.Price;
        entity.Commission = model.Commission;

        return entity;
    }

#pragma warning restore RMG020
#pragma warning restore RMG012
}
