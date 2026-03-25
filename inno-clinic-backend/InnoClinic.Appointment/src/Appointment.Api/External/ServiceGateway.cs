using Appointment.Api.Common;

namespace Appointment.Api.External;

public class ServiceGateway(IHttpClientFactory httpClientFactory)
: IServiceGateway
{
    public async Task<Result<bool>> IsServiceActiveAsync(Guid serviceId, CancellationToken cancellationToken = default)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetFromJsonAsync<bool>(
                $"http://clinic-management-api/services/{serviceId}/active",
                cancellationToken);

            return Result<bool>.Success(response);
        }
        catch (HttpRequestException)
        {
            return Errors.ServiceIsNotActive;
        }
        catch (Exception)
        {
            return new Error("ServiceGateway.Error", "Unexpected error contactng Service Service");
        }
    }

    public async Task<TimeSpan?> GetServiceDurationAsync(Guid serviceId, CancellationToken cancellationToken = default)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient();
            var minutes = await httpClient.GetFromJsonAsync<int>(
                $"http://clinic-management-api/services/{serviceId}/duration-minutes",
                cancellationToken);

            if (minutes <= 0) return null;
            return TimeSpan.FromMinutes(minutes);
        }
        catch
        {
            return null;
        }
    }
}
