using Appointment.Api.Endpoints;

namespace Appointment.Api.Features.RemoveAppointment;

public class RemoveAppointmentEndpoint
{
    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("appointments/{id}", RemoveAppointmentHandler.HandleAsync)
                .WithTags("Appointments");
        }
    }
}
