using Apex.Invest.Domain.Entities.Bond;
using Apex.Invest.Factories;
using Apex.Invest.Features.Modules.Bonds.Details.Filters;
using Apex.Invest.Features.Modules.Bonds.Details.Mapper;
using Apex.Invest.Features.Modules.Bonds.Details.Models;
using Apex.Invest.Features.Modules.Bonds.Details.Queries;
using Apex.Invest.Modules;
using Apex.Invest.Persistance;
using Apex.Invest.Persistance.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Apex.Invest.Features.Modules.Bonds.Details.Services;

public class BondDetailsService(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory
) : BaseManageService<BondDetailsModel, Bond, BondDetailsFilter, BondDetailsQuery>(dbFactory, mapperFactory.GetMapper<BondDetailsMapper>())
{
    protected override string GetNotFoundMessage(object? context)
        => context switch
        {
            BondDetailsQuery q => $"Bond was not found {q.Id}",
            BondDetailsModel m => $"Bond was not found {m.Id}",
            _ => "Bond was not found"
        };

    protected override async Task<Bond> UpsertData(EntitiesDbContext db, BondDetailsModel model, CancellationToken cancellationToken)
        => await db.Set<Bond>()
            .Upsert(model)
            .OnKey(src => [src.Id])
            .CreateWhen(model => model.Id == 0)
            .ExecuteAsync((model, entity) => Mapper.MapData(model, entity), cancellationToken);
}
