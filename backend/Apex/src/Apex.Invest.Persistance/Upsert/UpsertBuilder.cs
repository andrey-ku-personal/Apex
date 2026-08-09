using Microsoft.EntityFrameworkCore;

namespace Apex.Invest.Persistance.Upsert;

public class UpsertBuilder<TEntity, TSource>(DbSet<TEntity> set, TSource source)
    where TEntity : class, new()
    where TSource : class
{
    private TEntity? _entity;
    private Func<TSource, bool>? _createWhenCondition;

    public UpsertBuilder<TEntity, TSource> OnKey(Func<TSource, object[]> keySelector)
    {
        var keys = keySelector(source);
        _entity = set.Find(keys);
        return this;
    }

    public UpsertBuilder<TEntity, TSource> CreateWhen(Func<TSource, bool> condition)
    {
        _createWhenCondition = condition;
        return this;
    }

    public async Task<TEntity> ExecuteAsync(Action<TSource, TEntity> updateFn, CancellationToken cancellationToken)
    {
        var isNew = _entity == null;

        if (isNew)
        {
            if (_createWhenCondition != null && !_createWhenCondition(source))
                throw new InvalidOperationException($"Cannot create {typeof(TEntity).Name} - create condition was not met.");

            _entity = new TEntity();
            await set.AddAsync(_entity, cancellationToken);
        }

        updateFn(source, _entity!);

        return _entity!;
    }
}
