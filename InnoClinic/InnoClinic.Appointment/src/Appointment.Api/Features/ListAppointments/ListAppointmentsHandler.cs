using Microsoft.EntityFrameworkCore;
using Appointment.Api.Data;
using Appointment.Api.External;

namespace Appointment.Api.Features.ListAppointment;

public sealed class ListAppointmentsHandler
{
    public async static Task<IResult> Handler(
        [AsParameters] ListAppointmentsRequest request,
        AppointmentDbContext context,
        IProfileGateway profileGateway,
        IServiceGateway serviceGateway)
    {
        var query = context.Appointments.AsNoTracking();

        if (request.DoctorId.HasValue)
            query = query.Where(a =>
                a.DoctorId == request.DoctorId.Value);

        if (request.PatientId.HasValue)
            query = query.Where(a =>
                a.PatientId == request.PatientId.Value);

        if (request.ServiceId.HasValue)
            query = query.Where(a =>
                a.ServiceId == request.ServiceId.Value);

        if (request.Date.HasValue)
            query = query.Where(a =>
                a.Date == request.Date.Value);

        if (request.DateStart.HasValue)
            query = query.Where(a =>
                a.Date >= request.DateStart.Value);

        if (request.DateEnd.HasValue)
            query = query.Where(a =>
                a.Date <= request.DateEnd.Value);

        if (request.IsApproved.HasValue)
            query = query.Where(a =>
                a.IsApproved == request.IsApproved.Value);

        var appointments = await query.ToListAsync();

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

        var responses = appointments.Select(a =>
        {
            var doctor = doctors.GetValueOrDefault(a.DoctorId);
            var patient = patients.GetValueOrDefault(a.PatientId);
            var service = services.GetValueOrDefault(a.ServiceId);

            return new ListAppointmentsResponse(
                Id:                 a.Id,
                PatientId:          a.PatientId,
                PatientFirstName:   patient?.FirstName ?? "Unknown",
                PatientLastName:    patient?.LastName ?? "Unknown",
                PatientMiddleName:  patient?.MiddleName ?? "Unknown",
                PatientPhoneNumber: patient?.PhoneNumber ?? "Unknown",
                DoctorId:           a.DoctorId,
                DoctorFirstName:    doctor?.FirstName ?? "Unknown",
                DoctorLastName:     doctor?.LastName ?? "Unknown",
                DoctorMiddleName:   doctor?.MiddleName ?? "Unknown",
                ServiceId:          a.ServiceId,
                ServiceName:        service?.Name ?? "Unknown",
                StartDateTime:      a.Date.ToDateTime(a.Time.Start),
                EndDateTime:        a.Date.ToDateTime(a.Time.End));
        });

        if (request.SortBy is "Date")
            responses = request.SortDirection is "Desc"
               ? responses.OrderByDescending(r => r.StartDateTime)
               : responses.OrderBy(r => r.StartDateTime);

        else if (request.SortBy is "DoctorName")
            responses = request.SortDirection is "Desc"
               ? responses
                    .OrderByDescending(r => r.DoctorLastName)
                    .ThenByDescending(r => r.DoctorFirstName)
               : responses
                    .OrderBy(r => r.DoctorLastName)
                    .ThenBy(r => r.DoctorFirstName);

        else if (request.SortBy is "ServiceName")
            responses = request.SortDirection is "Desc"
               ? responses
                    .OrderByDescending(r => r.ServiceName)
               : responses
                    .OrderBy(r => r.ServiceName);

        else
            responses = responses
               .OrderBy(r => r.StartDateTime)
               .ThenBy(r => r.DoctorLastName)
               .ThenBy(r => r.DoctorFirstName)
               .ThenBy(r => r.ServiceName);

        return TypedResults.Ok(responses.ToList());
    }
}
