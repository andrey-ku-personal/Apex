using Apex.Portfolio.Factories;
using Apex.Portfolio.Features.Modules.Bonds.Details.Filtering;
using Apex.Portfolio.Features.Modules.Bonds.Details.Models;
using Apex.Portfolio.Modules;
using Apex.Portfolio.Persistance;
using Microsoft.EntityFrameworkCore;
using BondEntity = Apex.Portfolio.Domain.Entities.Bond.Bond;

namespace Apex.Portfolio.Features.Modules.Bonds.Details.Services;

public class BondService(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory
) : BaseManageService<BondModel, BondEntity, BondFilter, BondQuery>(dbFactory, mapperFactory.GetMapper<BondModelMapper>())
{
    protected override string GetNotFoundMessage(object? context)
        => context is BondFilter filter
            ? $"Bond was not found {filter.Id}"
            : "Bond was not found";
}