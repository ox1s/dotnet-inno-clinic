using Appointment.Api.Data;
using Appointment.Api.Endpoints;

namespace Appointment.Api.Features.RemoveAppointment;

public sealed class RemoveAppointmentHandler
{
    public async static Task<IResult> HandleAsync(Guid id, AppointmentDbContext context)
    {
        var appointment = await context.Appointments.FindAsync(id);
        if (appointment is null) return Results.NotFound();

        context.Appointments.Remove(appointment);
        await context.SaveChangesAsync();

        return Results.Ok();
    }
}
