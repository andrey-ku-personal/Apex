using Apex.Portfolio.Factories;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Filtering;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Models;
using Apex.Portfolio.Modules;
using Apex.Portfolio.Persistance;
using Microsoft.EntityFrameworkCore;
using InstrumentMetadataEntity = Apex.Portfolio.Domain.Abstractions.InstrumentMetadata;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.Details.Services;

public class InstrumentMetadataService(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory
) : BaseManageService<InstrumentMetadataModel, InstrumentMetadataEntity, InstrumentMetadataFilter, InstrumentMetadataQuery>(dbFactory, mapperFactory.GetMapper<InstrumentMetadataModelMapper>())
{
    protected override string GetNotFoundMessage(object? context)
        => context switch
        {
            InstrumentMetadataQuery q => $"InstrumentMetadata was not found {q.Id}",
            InstrumentMetadataModel m => $"InstrumentMetadata was not found {m.Id}",
            InstrumentMetadataFilter f => $"InstrumentMetadata was not found {f.Id}",
            _ => "InstrumentMetadata was not found"
        };
}