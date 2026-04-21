using Appointment.Api.Endpoints;

using InnoClinic.Shared;

namespace Appointment.Api.Features.Appointments.CancelAppointment;

public class CancelAppointmentEndpoint
{
    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("appointments/{id}", CancelAppointmentHandler.HandleAsync)
                .WithTags("Appointments")
                .RequireAuthorization(policy => policy.RequireRole(Roles.Receptionist));
        }
    }
}