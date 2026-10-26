using Apex.Portfolio.Factories;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Filtering;
using Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Models;
using Apex.Portfolio.Modules;
using Apex.Portfolio.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Services;

public class InstrumentMetadataListService(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory
) : BaseGetListService<
        InstrumentMetadataListModel,
        Domain.Abstractions.InstrumentMetadata,
        InstrumentMetadataListFilter,
        InstrumentMetadataListQuery
    >(dbFactory, mapperFactory.GetMapper<InstrumentMetadataListModelMapper>()), IInstrumentMetadataListService;