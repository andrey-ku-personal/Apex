using Apex.Shared.Core.Pagination.Abstract;
using Apex.Shared.Models.Pagination;

namespace Apex.Invest.Abstracts.List;

public interface IBaseListService<TModel, TEntity, TFilter, TQuery>
    where TModel : class
    where TEntity : class
    where TFilter : IPageSortQuery
    where TQuery : IBaseSortQuery<TEntity>, new()
{
    Task<PageDataResponse<TModel>> GetList(TFilter filter, CancellationToken cancellationToken);
}
