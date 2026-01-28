namespace Appointment.Api.Features.UpdateAppointment;

public record UpdateAppointmentRequest(
        Guid PatientId,
        Guid DoctorId,
        Guid ServiceId,
        Guid OfficeId,
        DateTime StartDateTime,
        DateTime EndDateTime);





