using Apex.Shared.Core.Pagination;
using System.Linq.Expressions;

namespace Apex.Invest.Features.Modules.Deposits.Details.Queries;

public class DepositDetailsQuery : BaseQuery<Apex.Invest.Domain.Entities.Deposit>
{
    public int Id { get; set; }

    public override Expression<Func<Apex.Invest.Domain.Entities.Deposit, bool>> GetExpression()
        => deposit => deposit.Id == Id;
}
