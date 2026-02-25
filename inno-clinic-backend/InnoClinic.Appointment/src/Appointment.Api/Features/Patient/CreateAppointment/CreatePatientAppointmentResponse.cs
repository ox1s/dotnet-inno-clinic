namespace Appointment.Api.Features.CreateAppointment;

public record CreatePatientAppointmentResponse(
    Guid Id,
    Guid PatientId,
    Guid DoctorId,
    Guid ServiceId,
    Guid OfficeId,
    DateTimeOffset StartDateTime,
    DateTimeOffset EndDateTime);