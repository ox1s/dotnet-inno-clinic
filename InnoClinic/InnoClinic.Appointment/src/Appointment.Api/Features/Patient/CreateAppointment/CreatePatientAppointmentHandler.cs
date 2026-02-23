using System.Security.Claims;

using Appointment.Api.Common;
using Appointment.Api.Data;
using Appointment.Api.External;
using Appointment.Api.Features.CreateAppointment;

using FluentValidation;

using InnoClinic.Shared.DTOs;

using Throw;
namespace Appointment.Api.Features.Patient.CreateAppointment;

public class CreatePatientAppointmentHandler
{
    public static async Task<IResult> HandleAsync(
        CreatePatientAppointmentRequest request,
        IAppointmentRepository appointmentRepository,
        IValidator<CreatePatientAppointmentRequest> validator,
        ClaimsPrincipal user,
        IServiceGateway serviceGateway,
        IOfficeGateway officeGateway,
        IProfileGateway profileGateway,
        IHttpClientFactory httpClientFactory,
        ILogger<CreatePatientAppointmentRequest> logger)
    {

        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Results.BadRequest(new Error(
                Code: "Validation",
                Description: validationResult.ToString()));

        var patientIdClaim = user.FindFirst("id");
        if (patientIdClaim == null || !Guid.TryParse(patientIdClaim.Value, out var patientId))
            return Results.Unauthorized();


        var timeRangeResult = TimeRange.Create(
            request.StartDateTime.ToUniversalTime(),
            request.EndDateTime.ToUniversalTime());


        timeRangeResult.ThrowIfNull();
        if (!timeRangeResult.Succeeded)
            return Results.BadRequest(timeRangeResult.Error.Description);

        if (await appointmentRepository.IsOverlappingAsync(request.DoctorId, timeRangeResult.Value!))
            return Results.BadRequest(Errors.OverlappingAppointment);

        var serviceActive = serviceGateway.IsServiceActiveAsync(request.ServiceId);
        var officeActive = officeGateway.IsOfficeActiveAsync(request.OfficeId);
        var doctorActiveResult = profileGateway.IsDoctorActiveAsync(request.DoctorId);

        // var checkServiceStatusRequest = new CheckStatus(request.ServiceId);
        // var checkServiceStatusResult = await CheckServiceStatus(
        //     request: checkServiceStatusRequest,
        //     httpClientFactory: httpClienFactory,
        //     logger: logger);

        // var checkOfficeStatusRequest = new CheckStatus(request.OfficeId);
        // var checkOfficeStatusResult = await CheckOfficeStatus(
        //     request: checkOfficeStatusRequest,
        //     httpClientFactory: httpClienFactory,
        //     logger: logger);

        // var checkDoctorStatusRequest = new CheckStatus(request.DoctorId);
        // var checkDoctorStatusResult = await CheckDoctorStatus(
        //     request: checkDoctorStatusRequest,
        //     httpClientFactory: httpClienFactory,
        //     logger: logger);

        if (!doctorActiveResult.Result)
            return Results.BadRequest(Errors.DoctorIsNotActive);
        if (!serviceActive.Result)
            return Results.BadRequest(Errors.ServiceIsNotActive);
        if (!officeActive.Result)
            return Results.BadRequest(Errors.OfficeIsNotActive);

        var appointment = Data.Appointment.Create(
            patientId: patientId,
            doctorId: request.DoctorId,
            serviceId: request.ServiceId,
            officeId: request.OfficeId,
            duration: timeRangeResult.Value!);

        await appointmentRepository.AddAsync(appointment);

        return Results.Ok(
            new CreatePatientAppointmentResponse(
                Id: appointment.Id,
                PatientId: appointment.PatientId,
                DoctorId: appointment.DoctorId,
                ServiceId: appointment.ServiceId,
                OfficeId: appointment.OfficeId,
                StartDateTime: appointment.Duration.Start,
                EndDateTime: appointment.Duration.End));
    }
    private static async Task<bool> CheckServiceStatus(
           CheckStatus request,
           IHttpClientFactory httpClientFactory,
           ILogger<CreatePatientAppointmentRequest> logger)
    {
        try
        {
            var client = httpClientFactory.CreateClient("ClinicManagementApi");
            var response = await client.GetAsync($"services/{request.EntityId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<CheckStatusResponse>();
                return json?.Status == "Active";
            }

            logger.LogWarning(
                "Failed to check service status. Status code: {StatusCode}",
                response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error check service status");
            return false;
        }
    }
    private static async Task<bool> CheckOfficeStatus(
       CheckStatus request,
       IHttpClientFactory httpClientFactory,
       ILogger<CreatePatientAppointmentRequest> logger)
    {
        try
        {
            var client = httpClientFactory.CreateClient("ClinicManagementApi");
            var response = await client.GetAsync($"offices/{request.EntityId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<CheckStatusResponse>();
                return json?.Status == "Active";
            }

            logger.LogWarning(
                "Failed to check office status. Status code: {StatusCode}",
                response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error check office status");
            return false;
        }
    }
    private static async Task<bool> CheckDoctorStatus(
       CheckStatus request,
       IHttpClientFactory httpClientFactory,
       ILogger<CreatePatientAppointmentRequest> logger)
    {
        try
        {
            var client = httpClientFactory.CreateClient("ProfileApi");
            var response = await client.GetAsync($"doctors/{request.EntityId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<CheckStatusResponse>();
                return json?.Status == "At work";
            }

            logger.LogWarning(
                "Failed to check doctor status. Status code: {StatusCode}",
                response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error check doctor status");
            return false;
        }
    }
}