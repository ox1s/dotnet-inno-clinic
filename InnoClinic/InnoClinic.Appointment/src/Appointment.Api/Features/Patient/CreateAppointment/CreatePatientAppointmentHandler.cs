using System.Security.Claims;

using Appointment.Api.Common;
using Appointment.Api.Data;
using Appointment.Api.External;

using FluentValidation;

using Throw;

namespace Appointment.Api.Features.CreateAppointment;

public class CreatePatientAppointmentHandler
{
    public static async Task<IResult> HandleAsync(
        CreatePatientAppointmentRequest request,
        IAppointmentRepository appointmentRepository,
        IValidator<CreatePatientAppointmentRequest> validator,
        ClaimsPrincipal user,
        IServiceGateway serviceGateway,
        IOfficeGateway officeGateway,
        IProfileGateway profileGateway)
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

}