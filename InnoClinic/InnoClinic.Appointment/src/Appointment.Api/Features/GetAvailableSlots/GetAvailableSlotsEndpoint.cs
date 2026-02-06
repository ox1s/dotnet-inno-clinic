using Appointment.Api.Endpoints;

namespace Appointment.Api.Features.GetAvailableSlots;

public sealed class GetAvailableSlotsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("appointments/slots", GetAvailableSlotsHandler.Handle)
            .WithTags("Appointments")
            .WithName("GetAvailableSlots")
            .Produces<GetAvailableSlotsResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }
}