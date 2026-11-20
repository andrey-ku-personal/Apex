using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Exceptions;
using Apex.Shared.Core.Pagination;
using Apex.Shared.Core.Pagination.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Apex.Portfolio.Features.Modules;

public abstract class GetHandler<TModel, TEntity>(IDbContextFactory<EntitiesDbContext> dbFactory)
    where TEntity : class
{
    protected abstract BaseQuery<TEntity> GetQuery(int id);

    protected abstract TModel MapToModel(TEntity entity);

    public async Task<TModel> Handle(int id, CancellationToken cancellationToken)
    {
        Validate(id);

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var entity = await db.Set<TEntity>().AsNoTracking()
            .ByQuery(GetQuery(id))
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"{typeof(TEntity).Name} was not found {id}");

        return MapToModel(entity);
    }

    private void Validate(int id)
    {
        if (id < 1)
            throw new ValidationException(new Dictionary<string, string[]> { ["Id"] = ["'Id' must be greater than '0'."] });
    }
}
