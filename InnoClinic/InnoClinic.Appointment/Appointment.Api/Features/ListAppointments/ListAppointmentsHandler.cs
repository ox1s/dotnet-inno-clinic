using Microsoft.EntityFrameworkCore;

using Appointment.Api.Data;

namespace Appointment.Api.Features.ListAppointment;

public sealed class ListAppointmentsHandler
{
    public async static Task<IResult> Handler(AppointmentDbContext context)
    {
        var appointments = await context.Appointments.ToListAsync();

        var responses = appointments.Select(a =>
            new ListAppointmentsResponse(
                a.Id,
                a.PatientId,
                a.DoctorId,
                StartDateTime: a.Date.ToDateTime(a.Time.Start),
                EndDateTime: a.Date.ToDateTime(a.Time.End))).ToList();

        return TypedResults.Ok(responses);
    }
}
