using Apex.Shared.Core.Pagination;
using System.Linq.Expressions;

namespace Apex.Invest.Features.Modules.Shares.Details.Queries;

public class ShareDetailsQuery : BaseQuery<Apex.Invest.Domain.Entities.Share>
{
    public int Id { get; set; }

    public override Expression<Func<Apex.Invest.Domain.Entities.Share, bool>> GetExpression()
        => share => share.Id == Id;
}
