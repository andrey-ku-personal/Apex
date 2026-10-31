using Apex.Portfolio.Factories;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Filtering;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Models;
using Apex.Portfolio.Modules;
using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using InstrumentMetadataEntity = Apex.Portfolio.Domain.Abstractions.InstrumentMetadata;
using BondEntity = Apex.Portfolio.Domain.Entities.Bond.Bond;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Services;

public class InstrumentMetadataService(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory
) : BaseManageService<InstrumentMetadataModel, InstrumentMetadataEntity, InstrumentMetadataFilter, InstrumentMetadataQuery>(dbFactory, mapperFactory.GetMapper<InstrumentMetadataModelMapper>())
{
    protected override string GetNotFoundMessage(object? context)
        => context is InstrumentMetadataFilter filter
            ? $"InstrumentMetadata was not found {filter.Id}"
            : "InstrumentMetadata was not found";

    protected override async Task ValidateDeleteAsync(EntitiesDbContext db, InstrumentMetadataEntity entity, CancellationToken cancellationToken)
    {
        var usedByBonds = await db.Set<BondEntity>()
            .AnyAsync(x => x.CurrencyId == entity.Id || x.PaymentFrequencyId == entity.Id, cancellationToken);

        if (usedByBonds)
            throw new ConflictException("InstrumentMetadata is used by bonds");
    }
}