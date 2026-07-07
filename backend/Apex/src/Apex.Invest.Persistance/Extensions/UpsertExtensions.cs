using Apex.Invest.Persistance.Upsert;
using Microsoft.EntityFrameworkCore;

namespace Apex.Invest.Persistance.Extensions;

public static class UpsertExtensions
{
    public static UpsertBuilder<TEntity, TSource> Upsert<TEntity, TSource>(
        this DbSet<TEntity> set,
        TSource source
    )
    where TEntity : class, new()
    where TSource : class
        => new(set, source);
}
