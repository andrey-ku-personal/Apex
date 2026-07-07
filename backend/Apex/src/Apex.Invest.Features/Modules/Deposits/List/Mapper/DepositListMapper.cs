using Apex.Invest.Abstracts.List;
using Apex.Invest.Domain.Entities;
using Apex.Invest.Features.Modules.Deposits.List.Filters;
using Apex.Invest.Features.Modules.Deposits.List.Models;
using Apex.Invest.Features.Modules.Deposits.List.Queries;
using Riok.Mapperly.Abstractions;

namespace Apex.Invest.Features.Modules.Deposits.List.Mapper;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class DepositListMapper : IBaseListMapper<DepositListModel, Deposit, DepositListFilter, DepositListQuery>
{
#pragma warning disable RMG020
#pragma warning disable RMG012

    public partial List<DepositListModel> MapToModel(List<Deposit> entity);
    public partial DepositListQuery MapToQuery(DepositListFilter filter);

#pragma warning restore RMG020
#pragma warning restore RMG012
}
