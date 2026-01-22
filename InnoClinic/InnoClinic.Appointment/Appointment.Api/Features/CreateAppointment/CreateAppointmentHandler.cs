using FluentValidation;

using Appointment.Api.Common;
using Appointment.Api.Data;

namespace Appointment.Api.Features.CreateAppointment;

public class CreateAppointmentHandler
{
    public async static Task<IResult> HandleAsync(
        CreateAppointmentRequest request,
        AppointmentDbContext dbContext,
        IValidator<CreateAppointmentRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Results.BadRequest(new Error(
                Code: "Validation",
                Description: validationResult.ToString()));

        var date = DateOnly.FromDateTime(request.StartDateTime);
        var startTime = TimeOnly.FromDateTime(request.StartDateTime);
        var endTime = TimeOnly.FromDateTime(request.EndDateTime);

        var timeRangeResult = TimeRange.Create(startTime, endTime);

        if (!timeRangeResult.Succeeded)
        {
            return Results.BadRequest(timeRangeResult.Error.Description);
        }

        var appointment = Data.Appointment.Create(
            patientId: request.PatientId,
            doctorId: request.DoctorId,
            date: date,
            time: timeRangeResult.Value!
            );

        dbContext.Appointments.Add(appointment);

        await dbContext.SaveChangesAsync();

        return Results.Ok(
                    new CreateAppointmentResponse(
                Id: appointment.Id,
                PatientId: appointment.PatientId,
                DoctorId: appointment.DoctorId,
                StartDateTime: appointment.Date.ToDateTime(appointment.Time.Start),
                EndDateTime: appointment.Date.ToDateTime(appointment.Time.End)));
    }

}

