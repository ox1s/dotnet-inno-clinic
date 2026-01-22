namespace Appointment.Api.Features.UpdateAppointment;

public record UpdateAppointmentResponse(
        Guid Id,
        Guid PatientId,
        Guid DoctorId,
        DateTime StartDateTime,
        DateTime EndDateTime);





