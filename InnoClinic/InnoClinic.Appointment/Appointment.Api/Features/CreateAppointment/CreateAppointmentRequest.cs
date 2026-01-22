namespace Appointment.Api.Features.CreateAppointment;


public record CreateAppointmentRequest(
    Guid PatientId,
    Guid DoctorId,
    DateTime StartDateTime,
    DateTime EndDateTime);
