using Appointment.Api.Data;
using Appointment.Api.External;

namespace Appointment.Api.Features.Receptionist.ListAppointments;

public class ListReceptionistAppointmentsHandler
{
    public static async Task<IResult> Handle(
        [AsParameters] ListReceptionistAppointmentsRequest request,
        IAppointmentRepository appointmentRepository,
        IProfileGateway profileGateway,
        IServiceGateway serviceGateway)
    {
        var filter = new AppointmentFilter(
            page: request.Page,
            pageSize: request.PageSize,
            doctorId: request.DoctorId,
            patientId: null,
            serviceId: request.ServiceId,
            officeId: request.OfficeId,
            date: request.Date,
            dateStart: null,
            dateEnd: null,
            isApproved: request.IsApproved
        );

        var appointments = await appointmentRepository.SearchAsync(filter);
        var totalCount = await appointmentRepository.CountAsync(filter);

        var doctorIds = appointments.Select(a => a.DoctorId).Distinct();
        var patientIds = appointments.Select(a => a.PatientId).Distinct();
        var serviceIds = appointments.Select(a => a.ServiceId).Distinct();

        var doctorsTask = profileGateway.GetDoctorsAsync(doctorIds);
        var patientsTask = profileGateway.GetPatientsAsync(patientIds);
        var servicesTask = serviceGateway.GetServicesAsync(serviceIds);

        await Task.WhenAll(doctorsTask, patientsTask, servicesTask);

        var doctors = (await doctorsTask).ToDictionary(d => d.Id);
        var patients = (await patientsTask).ToDictionary(p => p.Id);
        var services = (await servicesTask).ToDictionary(s => s.Id);

        var responseList = appointments.Select(a =>
        {
            var doc = doctors.GetValueOrDefault(a.DoctorId);
            var pat = patients.GetValueOrDefault(a.PatientId);
            var srv = services.GetValueOrDefault(a.ServiceId);

            return new ListReceptionistAppointmentsResponse(
                Id: a.Id,
                TimeSlot: $"{a.Duration.Start:HH:mm} - {a.Duration.End:HH:mm}",
                DoctorFullName: FormatFullName(doc?.FirstName, doc?.LastName, doc?.MiddleName),
                PatientFullName: FormatFullName(pat?.FirstName, pat?.LastName, pat?.MiddleName),
                PatientPhoneNumber: pat?.PhoneNumber ?? "N/A",
                ServiceName: srv?.Name ?? "Unknown",
                IsApproved: a.IsApproved,
                TotalCount: totalCount
            );
        });

        // TODO: подумать над сортировкой 
        IEnumerable<ListReceptionistAppointmentsResponse> sortedResponses;

        if (request.SortBy == "DoctorName")
        {
            sortedResponses = request.SortDirection == "Desc"
                ? responseList.OrderByDescending(x => x.DoctorFullName).ThenByDescending(x => x.ServiceName)
                : responseList.OrderBy(x => x.DoctorFullName).ThenBy(x => x.ServiceName);
        }
        else
        {
            sortedResponses = responseList
                .OrderBy(x => x.TimeSlot)
                .ThenBy(x => x.DoctorFullName)
                .ThenBy(x => x.ServiceName);
        }

        return TypedResults.Ok(sortedResponses.ToList());
    }

    private static string FormatFullName(string? first, string? last, string? middle)
    {
        var parts = new[] { last, first, middle }.Where(s => !string.IsNullOrWhiteSpace(s));
        return string.Join(" ", parts);
    }
}