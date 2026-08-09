using Apex.Invest.Domain.Entities.Bond;
using Apex.Shared.Core.Pagination;
using System.Linq.Expressions;

namespace Apex.Invest.Features.Modules.Bonds.List.Queries;

public class BondListQuery : BaseSortQuery<Bond>
{
    public override Expression<Func<Bond, bool>> GetExpression()
    {
        var filter = base.GetExpression();

        return filter;
    }
}
