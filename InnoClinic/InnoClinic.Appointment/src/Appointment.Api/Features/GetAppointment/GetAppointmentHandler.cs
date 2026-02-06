using Appointment.Api.Data;

using Microsoft.AspNetCore.Http.HttpResults;

namespace Appointment.Api.Features.GetAppointment;

public static class GetAppointmentHandler
{

    public static async Task<Results<Ok<GetAppointmentResponse>, NotFound>> HandleAsync(Guid id, AppointmentDbContext context)
    {
        var appointment = await context.Appointments.FindAsync(id);

        if (appointment is null) return TypedResults.NotFound();

        return TypedResults.Ok(
            new GetAppointmentResponse(
                appointment.Id,
                appointment.PatientId,
                appointment.DoctorId,
                StartDateTime: appointment.Time.Start,
                EndDateTime: appointment.Time.End));
    }
}