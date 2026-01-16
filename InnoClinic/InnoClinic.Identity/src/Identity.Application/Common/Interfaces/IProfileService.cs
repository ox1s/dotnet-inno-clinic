using ErrorOr;

namespace Identity.Application.Common.Interfaces;

public interface IProfileService
{
    Task<ErrorOr<(string Role, string Status)>> GetProfileDataAsync(
        Guid accountId,
        CancellationToken cancellationToken);
}
