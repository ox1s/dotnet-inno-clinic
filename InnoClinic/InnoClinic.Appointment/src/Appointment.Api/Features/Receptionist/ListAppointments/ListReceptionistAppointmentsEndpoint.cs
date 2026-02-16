using Appointment.Api.Endpoints;

namespace Appointment.Api.Features.Receptionist.ListAppointments;

public class ListReceptionistAppointmentsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("receptionist/appointments", ListReceptionistAppointmentsHandler.Handle)
           .WithTags("Receptionist")
           .WithName("ListReceptionistAppointments")
           .WithDescription("US-13 View appointment list by receptionist");
    }
}