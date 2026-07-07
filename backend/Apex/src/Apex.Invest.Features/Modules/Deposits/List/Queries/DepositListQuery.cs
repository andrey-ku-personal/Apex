using Apex.Shared.Core.Pagination;
using System.Linq.Expressions;

namespace Apex.Invest.Features.Modules.Deposits.List.Queries;

public class DepositListQuery : BaseSortQuery<Apex.Invest.Domain.Entities.Deposit>
{
    public override Expression<Func<Apex.Invest.Domain.Entities.Deposit, bool>> GetExpression()
    {
        var filter = base.GetExpression();

        return filter;
    }
}
