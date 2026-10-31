using Apex.Portfolio.Abstracts.List;
using Apex.Shared.Core.Pagination.Abstract;
using Apex.Shared.Core.Services;
using Apex.Shared.Models.Pagination;

namespace Apex.Portfolio.Modules;

public class BaseGetListServiceDecorator<TModel, TEntity, TFilter, TQuery>(
    IBaseListService<TModel, TEntity, TFilter, TQuery> service,
    IValidationRunner validator
)
    where TModel : class
    where TEntity : class
    where TFilter : IPageSortQuery
    where TQuery : IBaseSortQuery<TEntity>
{
    public async Task<PageDataResponse<TModel>> GetList(TFilter filter, CancellationToken cancellationToken)
    {
        await validator.ValidateAsync(filter, cancellationToken);
        return await service.GetList(filter, cancellationToken);
    }
}
