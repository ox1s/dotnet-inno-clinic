using Appointment.Api.Endpoints;

using InnoClinic.Shared;

namespace Appointment.Api.Features.CreateAppointment;

public sealed class CreateAppointmentEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/appointments", CreateAppointmentHandler.HandleAsync)
            .WithTags("Appointments")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Patient));
    }
}