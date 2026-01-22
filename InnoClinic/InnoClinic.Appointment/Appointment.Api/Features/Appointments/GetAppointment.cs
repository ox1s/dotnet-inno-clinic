using Appointment.Api.Endpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Appointment.Api.Features.Appointments;

public static class GetAppointment
{
    public record Request(
        Guid Id);

    public record Response(
        Guid Id,
        Guid PatientId,
        Guid DoctorId,
        DateTime StartDateTime,
        DateTime EndDateTime);

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("appointments/{id}", Handler)
                .WithTags("Appointments");
        }
    }

    public async static Task<Results<Ok<Response>, NotFound>> Handler(Guid id, AppointmentDbContext context)
    {
        var appointment = await context.Appointments.FindAsync(id);

        if (appointment is null) return TypedResults.NotFound();

        return TypedResults.Ok(
            new Response(
                appointment.Id,
                appointment.PatientId,
                appointment.DoctorId,
                StartDateTime: appointment.Date.ToDateTime(appointment.Time.Start),
                EndDateTime: appointment.Date.ToDateTime(appointment.Time.End)));
    }
}
