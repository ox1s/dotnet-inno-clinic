using Appointment.Api.Common;
using Appointment.Api.Data;
using Appointment.Api.Features.Receptionist.ListAppointments;

namespace Appointment.Api.Features.Doctor.GetSchedule;

public static class GetScheduleHandler
{
    public record Request(DateOnly? Date);

    public record Response(
        Guid AppointmentId,
        DateTimeOffset Start,
        DateTimeOffset End,
        Guid PatientId,
        string PatientFullName,
        string ServiceName,
        bool IsApproved);

    public static async Task<IResult> Handle(
        DateOnly? date,
        IAppointmentRepository repository,
        ICurrentUserProvider currentUserProvider,
        CancellationToken cancellationToken)
    {
        var doctorId = currentUserProvider.GetUserId();
        if (doctorId is null)
        {
            return Results.Unauthorized();
        }

        var targetDate = date ?? DateOnly.FromDateTime(DateTime.UtcNow);

        var filter = new AppointmentFilter(
            page: 1,
            pageSize: 100,
            doctorId: doctorId,
            patientId: null,
            serviceId: null,
            officeId: null,
            date: targetDate,
            dateStart: null,
            dateEnd: null,
            isApproved: null,
            sortBy: SortOptions.Date,
            sortDirection: SortDirection.Asc);

        var appointments = await repository.SearchAsync(filter, cancellationToken);

        var response = appointments.ConvertAll(a => new Response(
            AppointmentId: a.AppointmentId,
            Start: a.DurationStart,
            End: a.DurationEnd,
            PatientId: a.PatientId,
            PatientFullName: $"{a.PatientLastName} {a.PatientFirstName} {a.PatientMiddleName}".Trim(),
            ServiceName: a.ServiceName,
            IsApproved: a.IsApproved
        ));

        return Results.Ok(response);
    }
}
