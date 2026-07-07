using Apex.Shared.Core.Pagination;
using System.Linq.Expressions;

namespace Apex.Invest.Features.Modules.Bonds.Details.Queries;

public class BondDetailsQuery : BaseQuery<Apex.Invest.Domain.Entities.Bond>
{
    public int Id { get; set; }

    public override Expression<Func<Apex.Invest.Domain.Entities.Bond, bool>> GetExpression()
        => bond => bond.Id == Id;
}
