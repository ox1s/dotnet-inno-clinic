namespace Appointment.Api.Features.GetAppointment;

public record GetAppointmentResponse(
    Guid Id,
    Guid PatientId,
    Guid DoctorId,
    DateTime StartDateTime,
    DateTime EndDateTime);

