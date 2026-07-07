using Apex.Invest.Abstracts.List;
using Apex.Invest.Domain.Entities;
using Apex.Invest.Features.Modules.Shares.List.Filters;
using Apex.Invest.Features.Modules.Shares.List.Models;
using Apex.Invest.Features.Modules.Shares.List.Queries;
using Riok.Mapperly.Abstractions;

namespace Apex.Invest.Features.Modules.Shares.List.Mapper;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class ShareListMapper : IBaseListMapper<ShareListModel, Share, ShareListFilter, ShareListQuery>
{
#pragma warning disable RMG020
#pragma warning disable RMG012

    public partial List<ShareListModel> MapToModel(List<Share> entity);
    public partial ShareListQuery MapToQuery(ShareListFilter filter);

#pragma warning restore RMG020
#pragma warning restore RMG012
}
