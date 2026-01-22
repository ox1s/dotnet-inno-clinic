namespace Appointment.Api.Features.UpdateAppointment;

public record UpdateAppointmentRequest(
        Guid PatientId,
        Guid DoctorId,
        DateTime StartDateTime,
        DateTime EndDateTime);





