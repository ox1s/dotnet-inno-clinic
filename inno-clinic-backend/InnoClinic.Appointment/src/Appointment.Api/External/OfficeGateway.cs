using Appointment.Api.Common;

namespace Appointment.Api.External;

public class OfficeGateway(IHttpClientFactory httpClientFactory)
: IOfficeGateway
{
    public async Task<Result<bool>> IsOfficeActiveAsync(Guid officeId, CancellationToken cancellationToken = default)
    {
        try

        {
            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetFromJsonAsync<bool>(
                $"http://office-api/offices/{officeId}/active",
                cancellationToken);

            return Result<bool>.Success(response);
        }
        catch (HttpRequestException)
        {
            return Errors.OfficeIsNotActive;
        }
        catch (Exception)
        {
            return new Error("OfficeGateway.Error", "Unexpected error contactng Office Service");
        }
    }
}