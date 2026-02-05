using FluentValidation;
using Appointment.Api.Common;
using Appointment.Api.Data;
using System.Security.Claims;
using Appointment.Api.External;
using Microsoft.EntityFrameworkCore;
using Throw;

namespace Appointment.Api.Features.CreateAppointment;

public class CreateAppointmentHandler
{
    public async static Task<IResult> HandleAsync(
        CreateAppointmentRequest request,
        AppointmentDbContext context,
        IValidator<CreateAppointmentRequest> validator,
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

        var timeRangeResult = TimeRange.Create(request.StartDateTime.ToUniversalTime(), request.EndDateTime.ToUniversalTime());
        timeRangeResult.ThrowIfNull();
        if (!timeRangeResult.Succeeded)
            return Results.BadRequest(timeRangeResult.Error.Description);

        var isOverlapping = context.TimeRanges
            .Any(a =>
                a.Start == timeRangeResult.Value!.Start
                && a.End == timeRangeResult.Value!.End);
        if (isOverlapping)
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
            time: timeRangeResult.Value!);

        context.Appointments.Add(appointment);

        await context.SaveChangesAsync();

        return Results.Ok(
            new CreateAppointmentResponse(
                Id: appointment.Id,
                PatientId: appointment.PatientId,
                DoctorId: appointment.DoctorId,
                ServiceId: appointment.ServiceId,
                OfficeId: appointment.OfficeId,
                StartDateTime: appointment.Time.Start,
                EndDateTime: appointment.Time.End));
    }

}
