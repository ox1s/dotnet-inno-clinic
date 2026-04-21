using System.Security.Claims;

using Appointment.Api.Common;
using Appointment.Api.Data;
using Appointment.Api.External;
using Appointment.Api.Features.CreateAppointment;

using FluentValidation;

using Throw;
namespace Appointment.Api.Features.Patient.CreateAppointment;

public static class CreatePatientAppointmentHandler
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

        var serviceActiveResult = await serviceGateway.IsServiceActiveAsync(request.ServiceId);
        var officeActiveResult = await officeGateway.IsOfficeActiveAsync(request.OfficeId);
        var doctorActiveResult = await profileGateway.IsDoctorActiveAsync(request.DoctorId);
        var profileLinkedResult = await profileGateway.IsProfileLinkedAsync(patientId);

        if (!doctorActiveResult.Succeeded)
            return Results.BadRequest(Errors.DoctorIsNotActive);
        if (!serviceActiveResult.Succeeded)
            return Results.BadRequest(Errors.ServiceIsNotActive);
        if (!officeActiveResult.Succeeded)
            return Results.BadRequest(Errors.OfficeIsNotActive);
        if (!profileLinkedResult.Succeeded)
            return Results.BadRequest(Errors.ProfileNotFound);
        if (!profileLinkedResult.Value)
            return Results.BadRequest(Errors.ProfileNotLinked);

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
}
