namespace Appointment.Api.Features.CreateAppointment;

public record CreateAppointmentResponse(
    Guid Id,
    Guid PatientId,
    Guid DoctorId,
    Guid ServiceId,
    Guid OfficeId,
    DateTimeOffset StartDateTime,
    DateTimeOffset EndDateTime);
