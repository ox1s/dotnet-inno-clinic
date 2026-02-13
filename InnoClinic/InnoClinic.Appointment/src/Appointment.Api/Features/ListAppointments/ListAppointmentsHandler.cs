using Appointment.Api.Data;
using Appointment.Api.External;

using Microsoft.EntityFrameworkCore;

namespace Appointment.Api.Features.ListAppointments;

public sealed class ListAppointmentsHandler
{
    public static async Task<IResult> Handler(
        [AsParameters] ListAppointmentsRequest request,
        IAppointmentRepository appointmentRepository,
        IProfileGateway profileGateway,
        IServiceGateway serviceGateway)
    {
        // TODO: Разобраться
        var filter = new AppointmentFilter(
            Page: request.Page,
            PageSize: request.PageSize,
            DoctorId: request.DoctorId,
            PatientId: request.PatientId,
            ServiceId: request.ServiceId,
            Date: request.Date,
            DateStart: request.DateStart,
            DateEnd: request.DateEnd,
            IsApproved: request.IsApproved
            );

        var appointments = await appointmentRepository.SearchAsync(filter);
        var totalCount = await appointmentRepository.CountAsync(filter);

        if (request.DoctorId.HasValue)
        {
            var doctorIds = await appointmentRepository
                .GetDoctorIdsByAppointmentAsync(request.DoctorId.Value);

            var doctorsTask = await profileGateway.GetDoctorsAsync(doctorIds);
        }
        if (request.PatientId.HasValue)
        {
            var patientIds = await appointmentRepository
                .GetPatientIdsByAppointmentAsync(request.PatientId.Value);

            var patientsTask = await profileGateway.GetPatientsAsync(patientIds);
        }
        if (request.ServiceId.HasValue)
        {
            var serviceIds = await appointmentRepository
                .GetServiceIdsByAppointmentAsync(request.ServiceId.Value);

            var servicesTask = await serviceGateway.GetServicesAsync(serviceIds);
        }

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
                Id: a.Id,
                PatientId: a.PatientId,
                PatientFirstName: patient?.FirstName ?? "Unknown",
                PatientLastName: patient?.LastName ?? "Unknown",
                PatientMiddleName: patient?.MiddleName ?? "Unknown",
                PatientPhoneNumber: patient?.PhoneNumber ?? "Unknown",
                DoctorId: a.DoctorId,
                DoctorFirstName: doctor?.FirstName ?? "Unknown",
                DoctorLastName: doctor?.LastName ?? "Unknown",
                DoctorMiddleName: doctor?.MiddleName ?? "Unknown",
                ServiceId: a.ServiceId,
                ServiceName: service?.Name ?? "Unknown",
                StartDateTime: a.Duration.Start,
                EndDateTime: a.Duration.End,
                TotalCount: totalCount
                );
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