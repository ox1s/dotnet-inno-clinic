namespace Appointment.Api.Features.ListAppointments;

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
    DateTimeOffset EndDateTime,
    int TotalCount
    );