using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

using Profile.Domain.Abstractions;

namespace Profile.Infrastructure.Database;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    // Both the sync and async entry points must be covered. Overriding only the async one
    // meant any call to the synchronous SaveChanges() hard-deleted the row, quietly
    // defeating the soft-delete design and the !IsDeleted query filter that depends on it.
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ConvertDeletesToSoftDeletes(eventData);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ConvertDeletesToSoftDeletes(eventData);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void ConvertDeletesToSoftDeletes(DbContextEventData eventData)
    {
        if (eventData.Context is null)
        {
            return;
        }

        IEnumerable<EntityEntry<ISoftDeletable>> entries =
            eventData
                .Context
                .ChangeTracker
                .Entries<ISoftDeletable>()
                .Where(e => e.State == EntityState.Deleted);

        foreach (EntityEntry<ISoftDeletable> softDeletable in entries)
        {
            softDeletable.State = EntityState.Modified;
            softDeletable.Entity.IsDeleted = true;
            softDeletable.Entity.DeletedOnUtc = DateTime.UtcNow;
        }
    }
}