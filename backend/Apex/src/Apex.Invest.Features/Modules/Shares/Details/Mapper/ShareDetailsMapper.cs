using Apex.Invest.Abstracts.Details;
using Apex.Invest.Domain.Entities;
using Apex.Invest.Features.Modules.Shares.Details.Filters;
using Apex.Invest.Features.Modules.Shares.Details.Models;
using Apex.Invest.Features.Modules.Shares.Details.Queries;
using Riok.Mapperly.Abstractions;

namespace Apex.Invest.Features.Modules.Shares.Details.Mapper;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class ShareDetailsMapper : IBaseDetailsMapper<ShareDetailsModel, Share, ShareDetailsFilter, ShareDetailsQuery>
{
#pragma warning disable RMG020
#pragma warning disable RMG012
    public partial ShareDetailsModel MapToModel(Share entity);
    public partial ShareDetailsFilter MapToFilter(Share entity);
    public partial ShareDetailsQuery MapToQuery(ShareDetailsFilter filter);

    public partial void UpdateEntity(ShareDetailsModel model, Share entity);

    public Share MapData(ShareDetailsModel model, Share entity)
    {
        UpdateEntity(model, entity);

        return entity;
    }

#pragma warning restore RMG020
#pragma warning restore RMG012
}
