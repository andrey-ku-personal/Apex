using System.Linq.Expressions;
using Apex.Portfolio.Domain.Enums;
using Apex.Shared.Core.Pagination;
using BondOperationEntity = Apex.Portfolio.Domain.Entities.Bond.BondOperation;

namespace Apex.Portfolio.Features.Modules.BondOperations.List.Filtering;

public class BondOperationListQuery : BaseSortQuery<BondOperationEntity>
{
    public string UserId { get; set; } = null!;
    public int? BondId { get; set; }
    public OperationType? Type { get; set; }
    public DateOnly? DateFrom { get; set; }
    public DateOnly? DateTo { get; set; }

    public override Expression<Func<BondOperationEntity, bool>> GetExpression()
        => operation =>
            operation.UserId == UserId &&
            (BondId == null || operation.BondId == BondId) &&
            (Type == null || operation.Type == Type) &&
            (DateFrom == null || operation.Date >= DateFrom) &&
            (DateTo == null || operation.Date <= DateTo);
}
