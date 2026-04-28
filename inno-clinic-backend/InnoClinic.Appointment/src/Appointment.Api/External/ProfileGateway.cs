using System.Text.Json;

using Appointment.Api.Common;

using InnoClinic.Shared.DTOs;

using Throw;

namespace Appointment.Api.External;

public class ProfileGateway(IHttpClientFactory httpClientFactory)
    : IProfileGateway
{
    public async Task<Result<DoctorDto>> GetDoctorAsync(Guid doctorId, CancellationToken cancellationToken = default)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetFromJsonAsync<DoctorDto>(
                $"http://profile-api/doctors/{doctorId}",
                cancellationToken);

            response.ThrowIfNull();

            return Result<DoctorDto>.Success(response);
        }
        catch (HttpRequestException)
        {
            return Errors.DoctorNotFound;
        }
        catch (Exception)
        {
            return new Error("ProfileGateway.Error", "Unexpected error contactng Profile Service");
        }
    }

    public async Task<Result<PatientDto>> GetPatientAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetFromJsonAsync<PatientDto>(
                $"http://profile-api/patients/{patientId}",
                cancellationToken);

            response.ThrowIfNull();

            return Result<PatientDto>.Success(response);
        }
        catch (HttpRequestException)
        {
            return Errors.ProfileNotFound;
        }
        catch (Exception)
        {
            return new Error("ProfileGateway.Error", "Unexpected error contactng Profile Service");
        }
    }

    public async Task<Result<bool>> IsDoctorActiveAsync(
        Guid doctorId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetFromJsonAsync<DoctorDto>(
                $"http://profile-api/doctors/{doctorId}/",
                cancellationToken);
            var isActive = response?.IsActive;

            return Result<bool>.Success(isActive ?? false);
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

    public async Task<Result<bool>> IsProfileLinkedAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetFromJsonAsync<bool>(
                $"http://profile-api/patients/{patientId}/is-linked",
                cancellationToken);

            return Result<bool>.Success(response);
        }
        catch (HttpRequestException)
        {
            return Errors.ProfileNotFound;
        }
        catch (Exception)
        {
            return new Error("ProfileGateway.Error", "Unexpected error contactng Profile Service");
        }
    }
}