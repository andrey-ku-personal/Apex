using System.Linq.Expressions;
using Apex.Portfolio.Factories;
using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Controllers;
using Apex.Shared.Core.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using InstrumentMetadataEntity = Apex.Portfolio.Domain.Abstractions.InstrumentMetadata;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.Details;

public class GetHandler(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory)
    : GetHandler<InstrumentMetadataModel, InstrumentMetadataEntity>(dbFactory)
{
    private readonly InstrumentMetadataMapper mapper = mapperFactory.GetMapper<InstrumentMetadataMapper>();

    protected override BaseQuery<InstrumentMetadataEntity> GetQuery(int id)
        => new InstrumentMetadataQuery { Id = id };

    protected override InstrumentMetadataModel MapToModel(InstrumentMetadataEntity entity)
        => mapper.MapToModel(entity);
}

public class GetController(GetHandler handler) : SharedController
{
    [HttpGet("InstrumentMetadata/Get/{id:int}")]
    [OpenApiOperation(
        operationId: "InstrumentMetadata.Get",
        summary: "Get InstrumentMetadata",
        description: "Get InstrumentMetadata by id")]
    [OpenApiTags("InstrumentMetadata")]
    public async Task<InstrumentMetadataModel> Get([FromRoute] int id, CancellationToken cancellationToken = default)
        => await handler.Handle(id, cancellationToken);
}

public class InstrumentMetadataQuery : BaseQuery<InstrumentMetadataEntity>
{
    public int Id { get; set; }

    public override Expression<Func<InstrumentMetadataEntity, bool>> GetExpression()
        => metadata => metadata.Id == Id;
}
