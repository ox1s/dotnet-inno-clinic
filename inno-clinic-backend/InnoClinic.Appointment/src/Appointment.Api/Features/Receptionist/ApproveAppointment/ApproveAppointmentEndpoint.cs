using Appointment.Api.Endpoints;

using InnoClinic.Shared;

namespace Appointment.Api.Features.Receptionist.ApproveAppointment;

public sealed class ApproveAppointmentEndpoint : IEndpoint
{

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("receptionist/appointments/{id}/approve", ApproveAppointmentHandler.HandleAsync)
            .WithTags("Receptionist")
            .WithName("ApproveAppointment")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Receptionist))
            .WithDescription("US-14 Approve appointment");
    }
}