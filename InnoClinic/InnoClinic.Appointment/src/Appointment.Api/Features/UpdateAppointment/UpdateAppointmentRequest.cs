namespace Appointment.Api.Features.UpdateAppointment;

public record UpdateAppointmentRequest(
        Guid PatientId,
        Guid DoctorId,
        Guid ServiceId,
        Guid OfficeId,
        DateTimeOffset StartDateTime,
        DateTimeOffset EndDateTime);





