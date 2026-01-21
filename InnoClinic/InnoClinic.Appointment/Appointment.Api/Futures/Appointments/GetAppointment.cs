using Appointment.Api.Endpoints;

using Microsoft.AspNetCore.Http.HttpResults;

namespace Appointment.Api.Futures.Appointments;

public static class GetAppointment
{
    public record Response(
        Guid Id,
        Guid PatientId,
        Guid DoctorId,
        DateOnly Date,
        TimeOnly Time);

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("appointments/{id}", Handler)
                .WithTags("Appointments");
        }
    }

    public async static Task<Results<Ok<Response>, NotFound>> Handler(int id, AppointmentDbContext context)
    {
        var appointment = await context.Appointments.FindAsync(id);

        if (appointment is null) return TypedResults.NotFound();

        return TypedResults.Ok(
            new Response(
                appointment.Id,
                appointment.PatientId,
                appointment.DoctorId,
                appointment.Date,
                appointment.Time));
    }
}
