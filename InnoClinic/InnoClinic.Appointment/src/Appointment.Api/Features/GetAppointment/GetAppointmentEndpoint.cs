using Appointment.Api.Endpoints;

namespace Appointment.Api.Features.GetAppointment;


public sealed class GetAppointmentEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("appointments/{id}", GetAppointmentHandler.HandleAsync)
            .WithTags("Appointments");
    }
}

