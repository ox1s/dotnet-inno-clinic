using Appointment.Api.Endpoints;

using InnoClinic.Shared;

namespace Appointment.Api.Features.ApproveAppointment;

public sealed class ApproveAppointmentEndpoint : IEndpoint
{

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/appointments/{id}/approve", ApproveAppointmentHandler.HandleAsync)
            .WithTags("Appointments")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Receptionist));
    }
}