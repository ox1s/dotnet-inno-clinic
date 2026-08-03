using System.Net.Http.Json;

using ErrorOr;

using Identity.Application.Common.Interfaces;

using InnoClinic.Shared.DTOs;

namespace Identity.Infrastructure.Services.Profile;

public class ProfileService(HttpClient httpClient) : IProfileService
{
    public async Task<ErrorOr<(string Role, string Status)>> GetProfileDataAsync(
        Guid accountId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // ERROR: Profile data doesn't exist, error is throwing all of time...
            var response = await httpClient.GetFromJsonAsync<ProfileDataDto>(
                $"{accountId}",
                cancellationToken);

            if (response is null)
            {
                return Error.NotFound("Profile not found");
            }

            return ErrorOrFactory.From((Role: response.Role, Status: response.Status));
        }
        catch
        {
            return Error.Failure(
                "ProfileService.Error",
                "Failed to retrieve profile data");
        }
    }
}