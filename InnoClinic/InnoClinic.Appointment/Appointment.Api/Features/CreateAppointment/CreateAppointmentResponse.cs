namespace Appointment.Api.Features.CreateAppointment;


public record CreateAppointmentResponse(
    Guid Id,
    Guid PatientId,
    Guid DoctorId,
    DateTime StartDateTime,
    DateTime EndDateTime);

