using Apex.Invest.Domain.Entities;
using Apex.Invest.Factories;
using Apex.Invest.Features.Modules.Shares.Details.Filters;
using Apex.Invest.Features.Modules.Shares.Details.Mapper;
using Apex.Invest.Features.Modules.Shares.Details.Models;
using Apex.Invest.Features.Modules.Shares.Details.Queries;
using Apex.Invest.Modules;
using Apex.Invest.Persistance;
using Apex.Invest.Persistance.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Apex.Invest.Features.Modules.Shares.Details.Services;

public class ShareDetailsService(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory
) : BaseManageService<ShareDetailsModel, Share, ShareDetailsFilter, ShareDetailsQuery>(dbFactory, mapperFactory.GetMapper<ShareDetailsMapper>())
{
    protected override string GetNotFoundMessage(object? context)
        => context switch
        {
            ShareDetailsQuery q => $"Share was not found {q.Id}",
            ShareDetailsModel m => $"Share was not found {m.Id}",
            _ => "Share was not found"
        };

    protected override async Task<Share> UpsertData(EntitiesDbContext db, ShareDetailsModel model, CancellationToken cancellationToken)
        => await db.Set<Share>()
            .Upsert(model)
            .OnKey(model.Id)
            .CreateWhen(model => model.Id == 0)
            .ExecuteAsync((model, entity) => Mapper.MapData(model, entity), cancellationToken);
}
