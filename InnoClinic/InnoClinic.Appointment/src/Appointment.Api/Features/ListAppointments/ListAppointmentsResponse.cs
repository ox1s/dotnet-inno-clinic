namespace Appointment.Api.Features.ListAppointment;

public record ListAppointmentsResponse(
    Guid Id,
    Guid PatientId,
    Guid DoctorId,
    DateTime StartDateTime,
    DateTime EndDateTime);
