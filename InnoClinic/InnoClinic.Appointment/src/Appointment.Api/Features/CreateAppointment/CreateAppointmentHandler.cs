using FluentValidation;

using Appointment.Api.Common;
using Appointment.Api.Data;

using System.Security.Claims;

namespace Appointment.Api.Features.CreateAppointment;

public class CreateAppointmentHandler
{
    public async static Task<IResult> HandleAsync(
        CreateAppointmentRequest request,
        AppointmentDbContext dbContext,
        IValidator<CreateAppointmentRequest> validator,
        ClaimsPrincipal user)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Results.BadRequest(new Error(
                Code: "Validation",
                Description: validationResult.ToString()));

        var patientIdClaim = user.FindFirst("id");
        if (patientIdClaim == null || !Guid.TryParse(patientIdClaim.Value, out var patientId))
            return Results.Unauthorized();

        var date = DateOnly.FromDateTime(request.StartDateTime);
        var startTime = TimeOnly.FromDateTime(request.StartDateTime);
        var endTime = TimeOnly.FromDateTime(request.EndDateTime);

        var timeRangeResult = TimeRange.Create(startTime, endTime);

        if (!timeRangeResult.Succeeded)
            return Results.BadRequest(timeRangeResult.Error.Description);

        var appointment = Data.Appointment.Create(
            patientId: patientId,
            doctorId: request.DoctorId,
            serviceId: request.ServiceId,
            officeId: request.OfficeId,
            date: date,
            time: timeRangeResult.Value!);

        dbContext.Appointments.Add(appointment);

        await dbContext.SaveChangesAsync();

        return Results.Ok(
                    new CreateAppointmentResponse(
                Id: appointment.Id,
                PatientId: appointment.PatientId,
                DoctorId: appointment.DoctorId,
                ServiceId: appointment.ServiceId,
                OfficeId: appointment.OfficeId,
                StartDateTime: appointment.Date.ToDateTime(appointment.Time.Start),
                EndDateTime: appointment.Date.ToDateTime(appointment.Time.End)));
    }

}

