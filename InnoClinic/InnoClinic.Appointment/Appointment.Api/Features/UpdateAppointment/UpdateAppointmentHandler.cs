
using Microsoft.AspNetCore.Http.HttpResults;
using FluentValidation;

using Appointment.Api.Data;
using Appointment.Api.Common;

namespace Appointment.Api.Features.UpdateAppointment;

public class UpdateAppointmentHandler
{
    public async static Task<Results<Ok<UpdateAppointmentResponse>, NotFound>> HandleAsync(Guid id,
        UpdateAppointmentRequest request,
        AppointmentDbContext context,
        IValidator<UpdateAppointmentRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return (Results<Ok<UpdateAppointmentResponse>, NotFound>)Results.BadRequest(new Error(
                Code: "Validation",
                Description: validationResult.ToString()));

        var appointmentToUpdate = await context.Appointments.FindAsync(id);
        if (appointmentToUpdate is null)
            return Result.Failure(new Error(
                Code: "NotFound",
                Description: "Appointment not found"));

        var date = DateOnly.FromDateTime(request.StartDateTime);
        var startTime = TimeOnly.FromDateTime(request.StartDateTime);
        var endTime = TimeOnly.FromDateTime(request.EndDateTime);

        var timeRangeResult = TimeRange.Create(startTime, endTime);

        if (!timeRangeResult.Succeeded)
        {
            return (Results<Ok<UpdateAppointmentResponse>, NotFound>)Results.BadRequest(timeRangeResult.Error.Description);
        }

        var appointment = Data.Appointment.Create(
            patientId: request.PatientId,
            doctorId: request.DoctorId,
            date: date,
            time: timeRangeResult.Value!);

        await context.SaveChangesAsync();

        return TypedResults.Ok(
            new UpdateAppointmentResponse(
                appointment.Id,
                appointment.PatientId,
                appointment.DoctorId,
                StartDateTime: appointment.Date.ToDateTime(appointment.Time.Start),
                EndDateTime: appointment.Date.ToDateTime(appointment.Time.End)));
    }
}
