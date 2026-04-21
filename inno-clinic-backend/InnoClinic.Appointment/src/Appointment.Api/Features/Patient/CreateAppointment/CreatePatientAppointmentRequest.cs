namespace Appointment.Api.Features.CreateAppointment;

public record CreatePatientAppointmentRequest(
    Guid DoctorId,
    Guid ServiceId,
    Guid OfficeId,
    DateTimeOffset StartDateTime,
    DateTimeOffset EndDateTime);