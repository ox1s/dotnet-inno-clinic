using Appointment.Api.Endpoints;

namespace Appointment.Api.Features.CreateAppointment;

public sealed class CreateAppointmentEndpoint : IEndpoint
{

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/appointments", CreateAppointmentHandler.HandleAsync)
            .WithTags("Appointments");
    }
}
