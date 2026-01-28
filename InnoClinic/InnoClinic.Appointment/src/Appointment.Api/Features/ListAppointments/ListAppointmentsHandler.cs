using Microsoft.EntityFrameworkCore;

using Appointment.Api.Data;

namespace Appointment.Api.Features.ListAppointment;

public sealed class ListAppointmentsHandler
{
    public async static Task<IResult> Handler(
        [AsParameters] ListAppointmentsRequest request,
        AppointmentDbContext context)
    {
        var query = context.Appointments.AsNoTracking();

        if (request.DoctorId.HasValue)
        {
            query = query
                .Where(a => a.DoctorId == request.DoctorId.Value);
        }

        if (request.PatientId.HasValue)
        {
            query = query
                .Where(a => a.PatientId == request.PatientId.Value);
        }

        if (request.ServiceId.HasValue)
        {
            query = query
                .Where(a => a.ServiceId == request.ServiceId.Value);
        }

        if (request.Date.HasValue)
        {
            query = query
                .Where(a => a.Date == request.Date.Value);
        }

        if (request.IsApproved.HasValue)
        {
            query = query
                .Where(a => a.IsApproved == request.IsApproved.Value);
        }

        var appointments = await query.ToListAsync();

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
