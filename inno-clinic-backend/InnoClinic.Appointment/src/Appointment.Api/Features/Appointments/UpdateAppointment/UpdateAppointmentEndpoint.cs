using Appointment.Api.Endpoints;

using InnoClinic.Shared;

namespace Appointment.Api.Features.Appointments.UpdateAppointment;

public sealed class UpdateAppointmentEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/appointments/{id}", UpdateAppointmentHandler.HandleAsync)
            .WithTags("Appointments")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Patient, Roles.Receptionist));
    }
}