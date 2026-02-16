namespace Appointment.Api.Features.Appointments.UpdateAppointment;

public record UpdateAppointmentResponse(
    Guid Id,
    Guid PatientId,
    Guid DoctorId,
    Guid ServiceId,
    Guid OfficeId,
    DateTimeOffset StartDateTime,
    DateTimeOffset EndDateTime);