using Apex.Invest.Domain.Entities;
using Apex.Invest.Factories;
using Apex.Invest.Features.Modules.Deposits.Details.Filters;
using Apex.Invest.Features.Modules.Deposits.Details.Mapper;
using Apex.Invest.Features.Modules.Deposits.Details.Models;
using Apex.Invest.Features.Modules.Deposits.Details.Queries;
using Apex.Invest.Modules;
using Apex.Invest.Persistance;
using Apex.Invest.Persistance.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Apex.Invest.Features.Modules.Deposits.Details.Services;

public class DepositDetailsService(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory
) : BaseManageService<DepositDetailsModel, Deposit, DepositDetailsFilter, DepositDetailsQuery>(dbFactory, mapperFactory.GetMapper<DepositDetailsMapper>())
{
    protected override string GetNotFoundMessage(object? context)
        => context switch
        {
            DepositDetailsQuery q => $"Deposit was not found {q.Id}",
            DepositDetailsModel m => $"Deposit was not found {m.Id}",
            _ => "Deposit was not found"
        };

    protected override async Task<Deposit> UpsertData(EntitiesDbContext db, DepositDetailsModel model, CancellationToken cancellationToken)
        => await db.Set<Deposit>()
            .Upsert(model)
            .OnKey(model.Id)
            .CreateWhen(model => model.Id == 0)
            .ExecuteAsync((model, entity) => Mapper.MapData(model, entity), cancellationToken);
}
