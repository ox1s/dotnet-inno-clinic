
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

        var appointment = await context.Appointments.FindAsync(id);
        if (appointment is null)
            return TypedResults.NotFound();

        var date = DateOnly.FromDateTime(request.StartDateTime);
        var startTime = TimeOnly.FromDateTime(request.StartDateTime);
        var endTime = TimeOnly.FromDateTime(request.EndDateTime);

        var timeRangeResult = TimeRange.Create(startTime, endTime);

        if (!timeRangeResult.Succeeded)
            return (Results<Ok<UpdateAppointmentResponse>, NotFound>)Results.BadRequest(timeRangeResult.Error.Description);

        var existingAppointment = context.Appointments
            .Where(a => a.DoctorId == request.DoctorId && a.Date == date && a.Id != id)
            .AsEnumerable()
            .FirstOrDefault(a => a.Time.Overlaps(timeRangeResult.Value!));

        if (existingAppointment is not null)
            return (Results<Ok<UpdateAppointmentResponse>, NotFound>)Results.BadRequest(Errors.OverlappingAppointment);

        appointment.Update(
            doctorId: request.DoctorId,
            serviceId: request.ServiceId,
            officeId: request.OfficeId,
            date: date,
            time: timeRangeResult.Value!);

        await context.SaveChangesAsync();

        return TypedResults.Ok(
            new UpdateAppointmentResponse(
                appointment.Id,
                appointment.PatientId,
                appointment.DoctorId,
                appointment.ServiceId,
                appointment.OfficeId,
                StartDateTime: appointment.Date.ToDateTime(appointment.Time.Start),
                EndDateTime: appointment.Date.ToDateTime(appointment.Time.End)));
    }
}
