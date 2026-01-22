namespace Appointment.Api.Features.UpdateAppointment;

public record UpdateAppointmentResponse(
    Guid Id,
    Guid PatientId,
    Guid DoctorId,
    Guid ServiceId,
    Guid OfficeId,
    DateTime StartDateTime,
    DateTime EndDateTime);
