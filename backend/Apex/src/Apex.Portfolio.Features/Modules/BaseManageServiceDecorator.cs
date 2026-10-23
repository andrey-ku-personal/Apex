using Apex.Portfolio.Abstracts.Details;
using Apex.Portfolio.Services;
using Apex.Shared.Core.Pagination.Abstract;

namespace Apex.Portfolio.Modules;

public class BaseManageServiceDecorator<TModel, TEntity, TFilter, TQuery>(
    IBaseDetailsService<TModel, TEntity, TFilter> service,
    IValidationRunner validator
)
    where TModel : class
    where TEntity : class
    where TFilter : class
    where TQuery : IBaseQuery<TEntity>
{
    public async Task<TModel> Upsert(TModel model, CancellationToken cancellationToken)
    {
        await validator.ValidateAsync(model, cancellationToken);
        return await service.Upsert(model, cancellationToken);
    }

    public async Task Delete(TFilter filter, CancellationToken cancellationToken)
    {
        await validator.ValidateAsync(filter, cancellationToken);
        await service.Delete(filter, cancellationToken);
    }

    public async Task<TModel> Get(TFilter filter, CancellationToken cancellationToken)
    {
        await validator.ValidateAsync(filter, cancellationToken);
        return await service.Get(filter, cancellationToken);
    }
}
