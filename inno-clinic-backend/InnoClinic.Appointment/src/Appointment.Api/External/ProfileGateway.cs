using Appointment.Api.Common;

namespace Appointment.Api.External;

public class ProfileGateway(IHttpClientFactory httpClientFactory)
    : IProfileGateway
{
    public async Task<Result<bool>> IsDoctorActiveAsync(
        Guid doctorId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetFromJsonAsync<bool>(
                $"http://profile-api/profiles/{doctorId}/active",
                cancellationToken);

            return Result<bool>.Success(response);
        }
        catch (HttpRequestException)
        {
            return Errors.DoctorIsNotActive;
        }
        catch (Exception)
        {
            return new Error("ProfileGateway.Error", "Unexpected error contactng Profile Service");
        }
    }
}