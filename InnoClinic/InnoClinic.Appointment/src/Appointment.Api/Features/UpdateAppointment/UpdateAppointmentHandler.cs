
using Microsoft.AspNetCore.Http.HttpResults;

using FluentValidation;

using Appointment.Api.Data;
using Appointment.Api.Common;
using Microsoft.EntityFrameworkCore;
using Throw;

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

        var appointment = await context.Appointments
            .Include(a => a.Time)
            .FirstOrDefaultAsync(a => a.Id == id);
        if (appointment is null)
            return TypedResults.NotFound();

        var timeRangeResult = TimeRange.Create(request.StartDateTime, request.EndDateTime);
        timeRangeResult.ThrowIfNull();
        if (!timeRangeResult.Succeeded)
            return (Results<Ok<UpdateAppointmentResponse>, NotFound>)Results.BadRequest(timeRangeResult.Error.Description);

        var isOverlapping = context.TimeRanges
            .Any(a =>
                a.Start == timeRangeResult.Value!.Start
                && a.End == timeRangeResult.Value!.End);
        if (isOverlapping)
            return (Results<Ok<UpdateAppointmentResponse>, NotFound>)Results.BadRequest(Errors.OverlappingAppointment);

        appointment.Update(
            doctorId: request.DoctorId,
            serviceId: request.ServiceId,
            officeId: request.OfficeId,
            time: timeRangeResult.Value!);

        await context.SaveChangesAsync();

        return TypedResults.Ok(
            new UpdateAppointmentResponse(
                Id: appointment.Id,
                PatientId: appointment.PatientId,
                DoctorId: appointment.DoctorId,
                ServiceId: appointment.ServiceId,
                OfficeId: appointment.OfficeId,
                StartDateTime: appointment.Time.Start,
                EndDateTime: appointment.Time.End));
    }
}
