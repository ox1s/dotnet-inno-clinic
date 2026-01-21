using Microsoft.EntityFrameworkCore;

using Appointment.Api.Endpoints;

namespace Appointment.Api.Futures.Appointments;

public static class ListAppointments
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
            app.MapGet("appointments", Handler).WithTags("Appointments");
        }
    }

    public async static Task<IResult> Handler(AppointmentDbContext context)
    {
        var appointments = await context.Appointments.ToListAsync();

        var responses = appointments.Select(a =>
            new Response(
                a.Id,
                a.PatientId,
                a.DoctorId,
                a.Date,
                a.Time)).ToList();

        return TypedResults.Ok(responses);
    }
}
