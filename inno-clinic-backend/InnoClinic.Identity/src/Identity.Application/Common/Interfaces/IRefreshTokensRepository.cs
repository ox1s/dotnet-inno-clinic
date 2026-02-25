using Identity.Domain.AccountAggregate;

namespace Identity.Application.Common.Interfaces;

public interface IRefreshTokensRepository
{
    Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    Task<RefreshToken?> GetRefreshTokenAsync(string queryRefreshToken, CancellationToken cancellationToken = default);
    Task RevokeRefreshTokensAsync(Guid accountId, CancellationToken cancellationToken = default);
}