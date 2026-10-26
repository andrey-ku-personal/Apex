using System.Linq.Expressions;
using Apex.Shared.Core.Pagination;
using BondEntity = Apex.Portfolio.Domain.Entities.Bond.Bond;

namespace Apex.Portfolio.Features.Modules.Bonds.Details.Filtering;

public class BondQuery : BaseQuery<BondEntity>
{
    public int Id { get; set; }

    public override Expression<Func<BondEntity, bool>> GetExpression()
        => bond => bond.Id == Id;
}