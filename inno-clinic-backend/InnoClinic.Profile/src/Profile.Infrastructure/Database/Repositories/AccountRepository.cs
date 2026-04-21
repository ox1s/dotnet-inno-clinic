using Profile.Domain.Entities.AccountProfiles;

namespace Profile.Infrastructure.Database.Repositories;

public class AccountRepository : Repository<AccountProfile>
{
    public AccountRepository(ProfileDbContext dbContext) : base(dbContext)
    {
    }
    public async Task<Guid?> GetEntityIdByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        return DbContext.Set<AccountProfile>()
            .Where(a => a.AccountId == accountId)
            .Select(a => a.Id)
            .FirstOrDefault();
    }
}