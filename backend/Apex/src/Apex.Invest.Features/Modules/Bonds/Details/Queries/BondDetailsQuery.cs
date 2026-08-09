using Apex.Invest.Domain.Entities.Bond;
using Apex.Shared.Core.Pagination;
using System.Linq.Expressions;

namespace Apex.Invest.Features.Modules.Bonds.Details.Queries;

public class BondDetailsQuery : BaseQuery<Bond>
{
    public int Id { get; set; }

    public override Expression<Func<Bond, bool>> GetExpression()
        => bond => bond.Id == Id;
}
