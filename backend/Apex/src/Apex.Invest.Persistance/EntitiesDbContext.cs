using Apex.Invest.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Apex.Invest.Persistance;

public class EntitiesDbContext(DbContextOptions<EntitiesDbContext> options) : DbContext(options)
{
    protected IDbContextTransaction? _currentTransaction;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EntitiesDbContext).Assembly);
    }

    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction != null) return;

        _currentTransaction = await Database.BeginTransactionAsync();
    }

    public void BeginTransaction()
    {
        if (_currentTransaction != null) return;

        _currentTransaction = Database.BeginTransaction();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            TrackChanges();
            await SaveChangesAsync().ConfigureAwait(false);
            if (_currentTransaction != null) await _currentTransaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            _currentTransaction?.Dispose();
            _currentTransaction = null;
        }
    }

    public void CommitTransaction()
    {
        try
        {
            TrackChanges();
            SaveChanges();
            _currentTransaction?.Commit();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            _currentTransaction?.Dispose();
            _currentTransaction = null;
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            _currentTransaction?.Dispose();
            _currentTransaction = null;
        }
    }

    protected void TrackChanges()
    {
        foreach (var entry in ChangeTracker.Entries<AnalyticalEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
            }
    }
}