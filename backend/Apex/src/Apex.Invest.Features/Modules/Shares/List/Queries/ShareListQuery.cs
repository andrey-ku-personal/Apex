using Apex.Shared.Core.Pagination;
using System.Linq.Expressions;

namespace Apex.Invest.Features.Modules.Shares.List.Queries;

public class ShareListQuery : BaseSortQuery<Apex.Invest.Domain.Entities.Share>
{
    public override Expression<Func<Apex.Invest.Domain.Entities.Share, bool>> GetExpression()
    {
        var filter = base.GetExpression();

        return filter;
    }
}
