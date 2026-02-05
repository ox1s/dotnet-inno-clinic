using Microsoft.AspNetCore.Http.HttpResults;

using Appointment.Api.Data;

namespace Appointment.Api.Features.GetAppointment;

public static class GetAppointmentHandler
{

    public async static Task<Results<Ok<GetAppointmentResponse>, NotFound>> HandleAsync(Guid id, AppointmentDbContext context)
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
