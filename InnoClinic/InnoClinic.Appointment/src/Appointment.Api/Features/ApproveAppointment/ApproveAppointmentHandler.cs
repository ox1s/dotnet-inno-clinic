using Appointment.Api.Data;

namespace Appointment.Api.Features.ApproveAppointment;

public class ApproveAppointmentHandler
{
    public async static Task<IResult> HandleAsync(
        Guid id,
        AppointmentDbContext context)
    {
        var appointment = await context.Appointments.FindAsync(id);
        if (appointment is null)
            return Results.NotFound();

        appointment.Approve();

        await context.SaveChangesAsync();

        return Results.Ok();
    }
}
