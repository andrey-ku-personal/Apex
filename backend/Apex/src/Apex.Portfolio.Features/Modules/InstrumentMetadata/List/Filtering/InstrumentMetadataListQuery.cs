using System.Linq.Expressions;
using Apex.Portfolio.Domain.Enums;
using Apex.Shared.Core.Pagination;

namespace Apex.Portfolio.Features.Modules.InstrumentMetadata.List.Filtering;

public class InstrumentMetadataListQuery : BaseSortQuery<Domain.Abstractions.InstrumentMetadata>
{
    public MetadataType? Type { get; set; }

    public string? FreeText { get; set; }

    public override Expression<Func<Domain.Abstractions.InstrumentMetadata, bool>> GetExpression()
        => metadata =>
            (Type == null || metadata.Type == Type) &&
            (string.IsNullOrWhiteSpace(FreeText) || metadata.Name.Contains(FreeText));
}