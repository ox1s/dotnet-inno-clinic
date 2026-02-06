namespace Appointment.Api.Features.ListAppointment;

public record ListAppointmentsResponse(
    Guid Id,
    Guid PatientId,
    string PatientFirstName,
    string PatientLastName,
    string PatientMiddleName,
    string PatientPhoneNumber,
    Guid DoctorId,
    string DoctorFirstName,
    string DoctorLastName,
    string DoctorMiddleName,
    Guid ServiceId,
    string ServiceName,
    DateTimeOffset StartDateTime,
    DateTimeOffset EndDateTime);