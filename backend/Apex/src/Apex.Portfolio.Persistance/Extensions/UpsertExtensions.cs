using Apex.Portfolio.Persistance.Upsert;
using Microsoft.EntityFrameworkCore;

namespace Apex.Portfolio.Persistance.Extensions;

public static class UpsertExtensions
{
    public static UpsertBuilder<TEntity, TSource> Upsert<TEntity, TSource>(this DbSet<TEntity> set, TSource source)
        where TEntity : class
        where TSource : class
        => new(set, source);
}
