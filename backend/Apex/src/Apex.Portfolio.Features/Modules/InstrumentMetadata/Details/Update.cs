using Apex.Portfolio.Factories;
using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Controllers;
using Apex.Shared.Core.Exceptions;
using Apex.Shared.Core.Pagination;
using Apex.Shared.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using InstrumentMetadataEntity = Apex.Portfolio.Domain.Abstractions.InstrumentMetadata;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.Details;

public class UpdateRequest : InstrumentMetadataModel;

public class UpdateRequestValidator : AbstractValidator<UpdateRequest>
{
    public UpdateRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(128);
        RuleFor(x => x.Type).IsInEnum();
    }
}

public class InstrumentMetadataUpdateMapper
{
    public InstrumentMetadataEntity MapData(UpdateRequest request, InstrumentMetadataEntity entity)
    {
        if (entity.Type != request.Type)
            throw new BadRequestException($"InstrumentMetadata type cannot be changed from '{entity.Type}' to '{request.Type}'.");

        entity.Name = request.Name;

        return entity;
    }
}

public class UpdateHandler(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory,
    IValidationRunner validation)
    : UpdateHandler<InstrumentMetadataModel, UpdateRequest, InstrumentMetadataEntity>(dbFactory, validation)
{
    private readonly InstrumentMetadataUpdateMapper updateMapper = mapperFactory.GetMapper<InstrumentMetadataUpdateMapper>();
    private readonly InstrumentMetadataMapper mapper = mapperFactory.GetMapper<InstrumentMetadataMapper>();

    protected override BaseQuery<InstrumentMetadataEntity> GetQuery(int id)
        => new InstrumentMetadataQuery { Id = id };

    protected override void MapData(UpdateRequest request, InstrumentMetadataEntity entity)
        => updateMapper.MapData(request, entity);

    protected override InstrumentMetadataModel MapToModel(InstrumentMetadataEntity entity)
        => mapper.MapToModel(entity);
}

public class UpdateController(UpdateHandler handler) : SharedController
{
    [HttpPut("InstrumentMetadata/Update/{id:int}")]
    [OpenApiOperation(
        operationId: "InstrumentMetadata.Update",
        summary: "Update InstrumentMetadata",
        description: "Update InstrumentMetadata name by id (Type is immutable)")]
    [OpenApiTags("InstrumentMetadata")]
    public async Task<InstrumentMetadataModel> Update([FromRoute] int id, [FromBody] UpdateRequest request, CancellationToken cancellationToken = default)
        => await handler.Handle(id, request, cancellationToken);
}
