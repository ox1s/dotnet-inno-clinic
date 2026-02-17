using Microsoft.EntityFrameworkCore;

using Appointment.Api.Data;

namespace Appointment.Api.Features.Receptionist.ListAppointments;

public class ListReceptionistAppointmentsHandler
{
    public static async Task<IResult> Handle(
        [AsParameters] ListReceptionistAppointmentsRequest request,
        IAppointmentRepository appointmentRepository,
        AppointmentDbContext dbContext)
    {

        var timeZoneId = Appointments_Resourses.Clinics_TimeZone ?? "UTC";
        var clinicTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

        var targetDate = request.Date ?? DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, clinicTimeZone));

        var localStart = targetDate.ToDateTime(TimeOnly.MinValue);
        var localEnd = localStart.AddDays(1);

        var utcStart = TimeZoneInfo.ConvertTimeToUtc(localStart, clinicTimeZone);
        var utcEnd = TimeZoneInfo.ConvertTimeToUtc(localEnd, clinicTimeZone);

        // var query = dbContext.AppointmentViews.AsNoTracking();

        // if (request.Date.HasValue)
        // {
        //     query = query.Where(x => x.DurationStart >= utcStart && x.DurationStart < utcEnd);
        // }

        // if (request.DoctorId.HasValue)
        //     query = query.Where(x => x.DoctorId == request.DoctorId);

        // if (request.ServiceId.HasValue)
        //     query = query.Where(x => x.ServiceId == request.ServiceId);

        // if (request.OfficeId.HasValue)
        //     query = query.Where(x => x.OfficeId == request.OfficeId);

        // if (request.IsApproved.HasValue)
        //     query = query.Where(x => x.IsApproved == request.IsApproved.Value);

        // query = request.SortBy switch
        // {
        //     "DoctorName" => request.SortDirection == "Desc"
        //         ? query.OrderByDescending(x => x.DoctorLastName).ThenByDescending(x => x.DoctorFirstName)
        //         : query.OrderBy(x => x.DoctorLastName).ThenBy(x => x.DoctorFirstName),

        //     "ServiceName" => request.SortDirection == "Desc"
        //         ? query.OrderByDescending(x => x.ServiceName)
        //         : query.OrderBy(x => x.ServiceName),

        //     _ => query.OrderBy(x => x.DurationStart)
        // };

        // var totalCount = await query.CountAsync();

        // var items = await query
        //     .Skip((request.Page - 1) * request.PageSize)
        //     .Take(request.PageSize)
        //     .ToListAsync();

        // var response = items.Select(x => new ListReceptionistAppointmentsResponse(
        //             Id: a.Id,
        //             TimeSlot: FormatTime(x.DurationStart, clinicTimeZone),
        //             DoctorFullName: FormatFullName(doc?.FirstName, doc?.LastName, doc?.MiddleName),
        //             PatientFullName: FormatFullName(pat?.FirstName, pat?.LastName, pat?.MiddleName),
        //             PatientPhoneNumber: pat?.PhoneNumber ?? "N/A",
        //             ServiceName: srv?.Name ?? "Unknown",
        //             IsApproved: a.IsApproved,
        //             TotalCount: totalCount
        //         )
        // );

        return TypedResults.Ok(/*response*/);
    }
    private static string FormatFullName(string? first, string? last, string? middle)
    {
        var parts = new[] { last, first, middle }.Where(s => !string.IsNullOrWhiteSpace(s));
        return string.Join(" ", parts);
    }
    private static string FormatTime(DateTimeOffset utcTime, TimeZoneInfo tz)
    {
        var localTime = TimeZoneInfo.ConvertTime(utcTime, tz);
        return $"{localTime:HH:mm}";
    }
}
