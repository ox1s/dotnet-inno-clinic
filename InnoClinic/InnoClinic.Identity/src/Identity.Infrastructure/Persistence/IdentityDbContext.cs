
using System.Reflection;
using Microsoft.EntityFrameworkCore;

using Identity.Application.Common.Interfaces;
using Identity.Domain.AccountAggregate;

namespace Identity.Infrastructure.Persistence;

public class IdentityDbContext(
    DbContextOptions<IdentityDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Account> Accounts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("identity");

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    // TODO: Domain Events
    public async Task CommitChangesAsync(CancellationToken cancellationToken = default)
    {
        await base.SaveChangesAsync(cancellationToken);
    }
}
