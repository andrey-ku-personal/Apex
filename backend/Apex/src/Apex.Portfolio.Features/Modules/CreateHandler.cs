using Apex.Portfolio.Features.Shared;
using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Apex.Portfolio.Features.Modules;

public abstract class CreateHandler<TModel, TRequest, TEntity>(
    IDbContextFactory<EntitiesDbContext> dbFactory,
    IValidationRunner validation)
    where TRequest : TModel
    where TEntity : class
{
    protected IValidationRunner Validation { get; } = validation;

    protected abstract TEntity MapToEntity(TRequest request);

    protected abstract TModel MapToModel(TEntity entity);


    public async Task<TModel> Handle(TRequest request, CancellationToken cancellationToken)
    {
        await Validate(request, cancellationToken);

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        await db.BeginTransactionAsync(cancellationToken);

        var entity = MapToEntity(request);
        await db.Set<TEntity>().AddAsync(entity, cancellationToken);

        await db.CommitOrConflictAsync(typeof(TEntity).Name, cancellationToken);

        return MapToModel(entity);
    }

    protected virtual Task Validate(TRequest request, CancellationToken cancellationToken)
        => Validation.ValidateAsync(request, cancellationToken);
}
