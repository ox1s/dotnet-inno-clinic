using Appointment.Api.Endpoints;

namespace Appointment.Api.Appointments;

public class RemoveAppointment
{
    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("appointments/{id}", Handler)
                .WithTags("Appointments");
        }
    }

    public async static Task<IResult> Handler(Guid id, AppointmentDbContext context)
    {
        var appointment = await context.Appointments.FindAsync(id);
        if (appointment is null) return Results.NotFound();

        context.Appointments.Remove(appointment);
        await context.SaveChangesAsync();

        return Results.Ok();
    }
}
