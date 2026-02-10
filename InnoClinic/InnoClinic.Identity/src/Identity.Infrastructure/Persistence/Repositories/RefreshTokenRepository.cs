using Microsoft.EntityFrameworkCore;

using Identity.Application.Common.Interfaces;
using Identity.Domain.AccountAggregate;

namespace Identity.Infrastructure.Persistence.Repositories;

public class RefreshTokensRepository(IdentityDbContext context)
    : IRefreshTokensRepository
{
    public async Task AddRefreshTokenAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default)
    {
        await context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
    }
    public async Task<RefreshToken?> GetRefreshTokenAsync(
        string queryRefreshToken,
        CancellationToken cancellationToken)
    {
        return await context.RefreshTokens
                   .Include(t => t.Account)
                   .FirstOrDefaultAsync(t => t.Token == queryRefreshToken, cancellationToken);
    }

    public async Task RevokeRefreshTokensAsync(
        Guid accountId,
        CancellationToken cancellationToken)
    {
        await context.RefreshTokens
            .Where(t => t.AccountId == accountId)
            .ExecuteDeleteAsync();
    }
}