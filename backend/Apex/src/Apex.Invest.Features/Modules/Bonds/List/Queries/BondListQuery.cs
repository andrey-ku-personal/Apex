using Apex.Shared.Core.Pagination;
using System.Linq.Expressions;

namespace Apex.Invest.Features.Modules.Bonds.List.Queries;

public class BondListQuery : BaseSortQuery<Apex.Invest.Domain.Entities.Bond>
{
    public override Expression<Func<Apex.Invest.Domain.Entities.Bond, bool>> GetExpression()
    {
        var filter = base.GetExpression();

        return filter;
    }
}
