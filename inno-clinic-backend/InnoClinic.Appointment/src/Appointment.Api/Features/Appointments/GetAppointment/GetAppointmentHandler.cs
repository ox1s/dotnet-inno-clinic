using Appointment.Api.Data;

using Microsoft.AspNetCore.Http.HttpResults;

namespace Appointment.Api.Features.Appointments.GetAppointment;

public static class GetAppointmentHandler
{
    public static async Task<Results<Ok<GetAppointmentResponse>, NotFound>> HandleAsync(
        Guid id,
        IAppointmentRepository appointmentRepository)
    {
        var appointment = await appointmentRepository.GetByIdAsync(id);

        if (appointment is null) return TypedResults.NotFound();

        return TypedResults.Ok(
            new GetAppointmentResponse(
                appointment.Id,
                appointment.PatientId,
                appointment.DoctorId,
                StartDateTime: appointment.Duration.Start,
                EndDateTime: appointment.Duration.End));
    }
}