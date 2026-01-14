using Identity.Application.Common.Interfaces;
using Identity.Domain.AccountAggregate;

namespace Identity.Infrastructure.Persistence.Repositories;

public class AccountsRepository(IdentityDbContext dbContext)
    : IAccountsRepository
{
    public async Task AddAccountAsync(Account user, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }

}
