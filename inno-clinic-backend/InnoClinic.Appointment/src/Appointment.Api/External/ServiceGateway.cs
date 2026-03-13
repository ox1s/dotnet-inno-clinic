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
                $"http://service-api/services/{serviceId}/active",
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

    public Task<TimeSpan?> GetServiceDurationAsync(Guid serviceId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}