using System.Linq.Expressions;
using Apex.Shared.Core.Pagination;
using BondOperationEntity = Apex.Portfolio.Domain.Entities.Bond.BondOperation;

namespace Apex.Portfolio.Features.Modules.BondOperations.Details.Filtering;

public class BondOperationQuery : BaseQuery<BondOperationEntity>
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;

    public override Expression<Func<BondOperationEntity, bool>> GetExpression()
        => operation => operation.Id == Id && operation.UserId == UserId;
}
