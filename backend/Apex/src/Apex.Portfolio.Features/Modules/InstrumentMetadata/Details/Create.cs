using Apex.Portfolio.Domain.Entities.InstrumentMetadata;
using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Factories;
using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Controllers;
using Apex.Shared.Core.Exceptions;
using Apex.Shared.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using InstrumentMetadataEntity = Apex.Portfolio.Domain.Abstractions.InstrumentMetadata;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.Details;

public class CreateRequest : InstrumentMetadataModel;

public class CreateRequestValidator : AbstractValidator<CreateRequest>
{
    public CreateRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(128);
        RuleFor(x => x.Type).IsInEnum();
    }
}

public class InstrumentMetadataCreateMapper
{
    public InstrumentMetadataEntity MapToEntity(CreateRequest request)
        => request.Type switch
        {
            MetadataType.Currency => new Currency { Name = request.Name },
            MetadataType.PaymentFrequency => new PaymentFrequency { Name = request.Name },
            _ => throw new BadRequestException($"InstrumentMetadata type '{request.Type}' is not supported.")
        };
}

public class CreateHandler(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory,
    IValidationRunner validation)
    : CreateHandler<InstrumentMetadataModel, CreateRequest, InstrumentMetadataEntity>(dbFactory, validation)
{
    private readonly InstrumentMetadataCreateMapper createMapper = mapperFactory.GetMapper<InstrumentMetadataCreateMapper>();
    private readonly InstrumentMetadataMapper mapper = mapperFactory.GetMapper<InstrumentMetadataMapper>();

    protected override InstrumentMetadataEntity MapToEntity(CreateRequest request)
        => createMapper.MapToEntity(request);

    protected override InstrumentMetadataModel MapToModel(InstrumentMetadataEntity entity)
        => mapper.MapToModel(entity);
}

public class CreateController(CreateHandler handler) : SharedController
{
    [HttpPost("InstrumentMetadata/Create")]
    [OpenApiOperation(
        operationId: "InstrumentMetadata.Create",
        summary: "Create InstrumentMetadata",
        description: "Create new InstrumentMetadata (Currency or PaymentFrequency)")]
    [OpenApiTags("InstrumentMetadata")]
    public async Task<InstrumentMetadataModel> Create([FromBody] CreateRequest request, CancellationToken cancellationToken = default)
        => await handler.Handle(request, cancellationToken);
}
