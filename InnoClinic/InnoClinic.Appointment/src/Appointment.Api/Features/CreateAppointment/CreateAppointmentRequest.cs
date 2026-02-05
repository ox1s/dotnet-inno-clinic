namespace Appointment.Api.Features.CreateAppointment;


public record CreateAppointmentRequest(
    Guid DoctorId,
    Guid ServiceId,
    Guid OfficeId,
    DateTimeOffset StartDateTime,
    DateTimeOffset EndDateTime);
