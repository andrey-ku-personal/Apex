using Apex.Portfolio.Abstracts.Details;
using Apex.Shared.Core.Pagination.Abstract;
using Apex.Shared.Core.Services;

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
    public async Task<TModel> Create(TModel model, CancellationToken cancellationToken)
    {
        await validator.ValidateAsync(model, cancellationToken);
        return await service.Create(model, cancellationToken);
    }

    public async Task<TModel> Update(TFilter filter, TModel model, CancellationToken cancellationToken)
    {
        await validator.ValidateAsync(filter, cancellationToken);
        await validator.ValidateAsync(model, cancellationToken);
        return await service.Update(filter, model, cancellationToken);
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
