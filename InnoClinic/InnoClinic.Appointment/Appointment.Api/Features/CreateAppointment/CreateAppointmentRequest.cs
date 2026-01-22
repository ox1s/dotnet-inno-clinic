namespace Appointment.Api.Features.CreateAppointment;


public record CreateAppointmentRequest(
    Guid DoctorId,
    Guid ServiceId,
    Guid OfficeId,
    DateTime StartDateTime,
    DateTime EndDateTime);
