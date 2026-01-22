using Appointment.Api.Endpoints;

namespace Appointment.Api.Features.ListAppointment;

public class ListAppointmentsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("appointments", ListAppointmentsHandler.Handler).WithTags("Appointments");
    }
}
