using Appointment.Api.Data;

namespace Appointment.Api.Features.Receptionist.ListAppointments;

public static class ListReceptionistAppointmentsHandler
{
    public static async Task<IResult> Handle(
        [AsParameters] ListReceptionistAppointmentsRequest request,
        IAppointmentRepository repository,
        CancellationToken cancellationToken)
    {
        var timeZoneId = Appointments_Resourses.Clinics_TimeZone ?? "UTC";

        TimeZoneInfo clinicTimeZone;
        try
        {
            clinicTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }
        catch
        {
            clinicTimeZone = TimeZoneInfo.Utc;
        }

        var targetDate = request.Date ?? DateOnly.FromDateTime(
            TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, clinicTimeZone));

        var sortBy = SortOptions.Date;
        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            Enum.TryParse(request.SortBy, ignoreCase: true, out sortBy);
        }

        var sortDirection = SortDirection.Asc;
        if (!string.IsNullOrWhiteSpace(request.SortDirection))
        {
            Enum.TryParse(request.SortDirection, ignoreCase: true, out sortDirection);
        }

        var filter = new AppointmentFilter(
            page: request.Page,
            pageSize: request.PageSize,
            doctorId: request.DoctorId,
            patientId: null,
            serviceId: request.ServiceId,
            officeId: request.OfficeId,
            date: targetDate,
            dateStart: null,
            dateEnd: null,
            isApproved: request.IsApproved,
            sortBy: sortBy,
            sortDirection: sortDirection
        );

        var totalCount = await repository.CountAsync(filter, cancellationToken);
        var appointments = await repository.SearchAsync(filter, cancellationToken);

        var response = appointments.Select(x => new ListReceptionistAppointmentsResponse(
            Id: x.AppointmentId,
            TimeSlot: FormatTime(x.DurationStart, x.DurationEnd, clinicTimeZone),
            DoctorFullName: FormatFullName(x.DoctorFirstName, x.DoctorLastName, x.DoctorMiddleName),
            PatientFullName: FormatFullName(x.PatientFirstName, x.PatientLastName, x.PatientMiddleName),
            PatientPhoneNumber: x.PatientPhone ?? "N/A",
            ServiceName: x.ServiceName,
            IsApproved: x.IsApproved,
            TotalCount: totalCount
        ));

        return TypedResults.Ok(response);
    }

    private static string FormatTime(
        DateTimeOffset start,
        DateTimeOffset end,
        TimeZoneInfo tz)
    {
        var localStart = TimeZoneInfo.ConvertTime(start, tz);
        var localEnd = TimeZoneInfo.ConvertTime(end, tz);

        return $"{localStart:HH:mm} - {localEnd:HH:mm}";
    }

    private static string FormatFullName(
        string? firstName,
        string? lastName,
        string? middleName)
    {
        return string.Join(" ",
            new[] { lastName, firstName, middleName }
            .Where(x => !string.IsNullOrWhiteSpace(x)));
    }
}