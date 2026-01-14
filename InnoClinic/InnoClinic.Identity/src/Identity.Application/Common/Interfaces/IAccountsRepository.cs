using Identity.Domain.AccountAggregate;

namespace Identity.Application.Common.Interfaces;

public interface IAccountsRepository
{
    Task AddAccountAsync(Account account, CancellationToken cancellationToken = default);

    // Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    // Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken = default);
    // Task<Account?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
    // Task<Account?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
