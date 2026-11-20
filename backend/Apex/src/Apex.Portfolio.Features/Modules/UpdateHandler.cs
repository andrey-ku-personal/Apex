using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Exceptions;
using Apex.Shared.Core.Pagination;
using Apex.Shared.Core.Pagination.Expressions;
using Apex.Shared.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Apex.Portfolio.Features.Modules;

public abstract class UpdateHandler<TModel, TRequest, TEntity>(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IValidationRunner validation)
    where TRequest : TModel
    where TEntity : class
{
    protected abstract BaseQuery<TEntity> GetQuery(int id);

    protected abstract void MapData(TRequest request, TEntity entity);

    protected abstract TModel MapToModel(TEntity entity);

    public async Task<TModel> Handle(int id, TRequest request, CancellationToken cancellationToken)
    {
        await Validate(id, request, cancellationToken);

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        await db.BeginTransactionAsync(cancellationToken);

        var entity = await db.Set<TEntity>()
            .ByQuery(GetQuery(id))
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"{typeof(TEntity).Name} was not found {id}");

        MapData(request, entity);

        await db.CommitTransactionAsync(cancellationToken);

        return MapToModel(entity);
    }

    private async Task Validate(int id, TRequest request, CancellationToken cancellationToken)
    {
        if (id < 1)
            throw new ValidationException(new Dictionary<string, string[]> { ["Id"] = ["'Id' must be greater than '0'."] });

        await validation.ValidateAsync(request, cancellationToken);
    }
}
