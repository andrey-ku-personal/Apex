using Apex.Invest.Abstracts.Details;
using Apex.Invest.Domain.Entities;
using Apex.Invest.Features.Modules.Deposits.Details.Filters;
using Apex.Invest.Features.Modules.Deposits.Details.Models;
using Apex.Invest.Features.Modules.Deposits.Details.Queries;
using Riok.Mapperly.Abstractions;

namespace Apex.Invest.Features.Modules.Deposits.Details.Mapper;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class DepositDetailsMapper : IBaseDetailsMapper<DepositDetailsModel, Deposit, DepositDetailsFilter, DepositDetailsQuery>
{
#pragma warning disable RMG020
#pragma warning disable RMG012
    public partial DepositDetailsModel MapToModel(Deposit entity);
    public partial DepositDetailsFilter MapToFilter(Deposit entity);
    public partial DepositDetailsQuery MapToQuery(DepositDetailsFilter filter);

    public partial void UpdateEntity(DepositDetailsModel model, Deposit entity);

    public Deposit MapData(DepositDetailsModel model, Deposit entity)
    {
        UpdateEntity(model, entity);

        return entity;
    }

#pragma warning restore RMG020
#pragma warning restore RMG012
}
