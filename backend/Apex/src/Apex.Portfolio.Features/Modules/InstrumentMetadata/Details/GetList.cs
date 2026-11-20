using System.Linq.Expressions;
using Apex.Portfolio.Domain.Enums;
using Apex.Portfolio.Factories;
using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Controllers;
using Apex.Shared.Core.Pagination;
using Apex.Shared.Core.Pagination.Expressions;
using Apex.Shared.Core.Pagination.Models;
using Apex.Shared.Core.Services;
using Apex.Shared.Models.Pagination;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using Riok.Mapperly.Abstractions;
using InstrumentMetadataEntity = Apex.Portfolio.Domain.Abstractions.InstrumentMetadata;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.Details;

public class GetListFilter : PageFilter
{
    public MetadataType? Type { get; set; }
}

public class GetListFilterValidator : AbstractValidator<GetListFilter>
{
    public GetListFilterValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.FreeText).MaximumLength(128);
    }
}

public class InstrumentMetadataListModel
{
    public int Id { get; set; }
    public MetadataType Type { get; set; }
    public string Name { get; set; } = default!;
}

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class InstrumentMetadataListMapper
{
#pragma warning disable RMG020
#pragma warning disable RMG012

    public partial List<InstrumentMetadataListModel> MapToModel(List<InstrumentMetadataEntity> entities);
    public partial InstrumentMetadataListQuery MapToQuery(GetListFilter filter);

#pragma warning restore RMG020
#pragma warning restore RMG012
}

public class GetListHandler(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IMapperFactory mapperFactory,
    IValidationRunner validation)
    : GetListHandler<InstrumentMetadataListModel, GetListFilter, InstrumentMetadataEntity, InstrumentMetadataListQuery>(dbFactory, validation)
{
    private readonly InstrumentMetadataListMapper mapper = mapperFactory.GetMapper<InstrumentMetadataListMapper>();

    protected override InstrumentMetadataListQuery MapToQuery(GetListFilter filter)
        => mapper.MapToQuery(filter);

    protected override List<InstrumentMetadataListModel> MapToModel(List<InstrumentMetadataEntity> entities)
        => mapper.MapToModel(entities);
}

public class GetListController(GetListHandler handler) : SharedController
{
    [HttpPost("InstrumentMetadata/Get/List")]
    [OpenApiOperation(
        operationId: "InstrumentMetadata.Get/List",
        summary: "Get List InstrumentMetadata",
        description: "Get paginated list of InstrumentMetadata (Currency, PaymentFrequency)")]
    [OpenApiTags("InstrumentMetadata")]
    public async Task<PageDataResponse<InstrumentMetadataListModel>> GetList([FromBody] GetListFilter filter, CancellationToken cancellationToken = default)
        => await handler.Handle(filter, cancellationToken);
}

public class InstrumentMetadataListQuery : BaseSortQuery<InstrumentMetadataEntity>
{
    public MetadataType? Type { get; set; }

    public string? FreeText { get; set; }

    public override Expression<Func<InstrumentMetadataEntity, bool>> GetExpression()
        => metadata =>
            (Type == null || metadata.Type == Type) &&
            (string.IsNullOrWhiteSpace(FreeText) || metadata.Name.Contains(FreeText));
}
