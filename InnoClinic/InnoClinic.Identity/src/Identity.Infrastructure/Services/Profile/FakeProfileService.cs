using ErrorOr;

using Identity.Application.Common.Interfaces;
using Identity.Domain.AccountAggregate;

namespace Identity.Infrastructure.Services.Profile;

public class FakeProfileService : IProfileService
{
    public Task<ErrorOr<(string Role, string Status)>> GetProfileDataAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            ErrorOrFactory.From((Role: Roles.Doctor, Status: "At work"))
        );

        // Для тестирования неактивного доктора

        // return Task.FromResult(
        //     ErrorOrFactory.From((Role: Roles.Doctor, Status: "Inactive"))
        // );

    }
}
