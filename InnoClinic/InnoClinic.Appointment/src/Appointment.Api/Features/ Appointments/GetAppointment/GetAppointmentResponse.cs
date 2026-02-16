namespace Appointment.Api.Features.Appointments.GetAppointment;

public record GetAppointmentResponse(
    Guid Id,
    Guid PatientId,
    Guid DoctorId,
    DateTimeOffset StartDateTime,
    DateTimeOffset EndDateTime);