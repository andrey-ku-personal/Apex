using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Apex.Portfolio.Features.Shared;

public static class EntitiesDbExtensions
{
    public static async Task CommitOrConflictAsync(
        this EntitiesDbContext db, string entityName, CancellationToken cancellationToken)
    {
        try
        {
            await db.CommitTransactionAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (exception.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new ConflictException($"{entityName} already exists");
        }
    }
}
