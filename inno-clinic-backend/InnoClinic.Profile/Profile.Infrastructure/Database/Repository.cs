using Microsoft.EntityFrameworkCore;

using Profile.Domain.Abstractions;

namespace Profile.Infrastructure.Database;

internal abstract class Repository<T>
    where T : Entity
{
    protected readonly ProfileDbContext DbContext;

    protected Repository(ProfileDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<T?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<T>()
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public virtual void Add(T entity)
    {
        DbContext.Add(entity);
    }
}