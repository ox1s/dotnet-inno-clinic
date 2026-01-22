using Appointment.Api.Endpoints;
using Microsoft.EntityFrameworkCore;

namespace Appointment.Api.Features.Appointments;

public static class ListAppointments
{
    private record Response(
        Guid Id,
        Guid PatientId,
        Guid DoctorId,
        DateTime StartDateTime,
        DateTime EndDateTime);

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
                StartDateTime: a.Date.ToDateTime(a.Time.Start),
                EndDateTime: a.Date.ToDateTime(a.Time.End))).ToList();

        return TypedResults.Ok(responses);
    }
}
