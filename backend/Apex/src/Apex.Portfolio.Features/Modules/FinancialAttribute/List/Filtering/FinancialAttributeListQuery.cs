using System.Linq.Expressions;
using Apex.Portfolio.Domain.Enums;
using Apex.Shared.Core.Pagination;

namespace Apex.Portfolio.Features.Modules.FinancialAttribute.List.Filtering;

public class FinancialAttributeListQuery : BaseSortQuery<Domain.Abstractions.FinancialAttribute>
{
    public MetadataType? Type { get; set; }

    public string? FreeText { get; set; }

    public override Expression<Func<Domain.Abstractions.FinancialAttribute, bool>> GetExpression()
        => attribute =>
            (Type == null || attribute.Type == Type) &&
            (string.IsNullOrWhiteSpace(FreeText) || attribute.Name.Contains(FreeText));
}
