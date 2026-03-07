using Appointment.Api.Data;

namespace Appointment.Api.Features.Appointments.CancelAppointment;

public static class CancelAppointmentHandler
{
    public static async Task<IResult> HandleAsync(
        Guid id,
        IAppointmentRepository appointmentRepository)
    {
        var appointment = await appointmentRepository.GetByIdAsync(id);
        if (appointment is null) return Results.NotFound();

        await appointmentRepository.DeleteAsync(appointment);

        return Results.Ok();
    }
}