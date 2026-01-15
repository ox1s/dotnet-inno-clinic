using Identity.Application.Common.Interfaces;
using Identity.Domain.AccountAggregate;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence.Repositories;

public class AccountsRepository(IdentityDbContext dbContext)
    : IAccountsRepository
{
    public async Task AddAccountAsync(Account account, CancellationToken cancellationToken = default)
    {
        await dbContext.Accounts.AddAsync(account, cancellationToken);
    }

    public async Task<Account?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Accounts
            .FirstOrDefaultAsync(a => a.Email == email, cancellationToken);
    }

    public Task UpdateAsync(Account account, CancellationToken cancellationToken = default)
    {
        dbContext.Accounts.Update(account);

        return Task.CompletedTask;
    }

    public async Task<bool> ExistsByIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Accounts
            .AnyAsync(a => a.Id == accountId, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Accounts
            .AnyAsync(a => a.Email == email, cancellationToken);
    }

    public async Task<Account?> GetByIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Accounts
            .FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken);

    }
}
