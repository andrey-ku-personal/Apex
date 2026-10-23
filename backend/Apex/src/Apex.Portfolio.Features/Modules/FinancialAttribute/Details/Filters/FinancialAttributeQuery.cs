using System.Linq.Expressions;
using Apex.Shared.Core.Pagination;

namespace Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Filters;

public class FinancialAttributeQuery : BaseQuery<Domain.Abstractions.FinancialAttribute>
{
    public int Id { get; set; }

    public override Expression<Func<Domain.Abstractions.FinancialAttribute, bool>> GetExpression()
        => attribute => attribute.Id == Id;
}
