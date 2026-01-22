using Microsoft.AspNetCore.Http.HttpResults;

using Appointment.Api.Common;
using Appointment.Api.Endpoints;

namespace Appointment.Api.Features.UpdateAppointment;

public sealed class UpdateAppointmentEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/appointments/{id}", UpdateAppointmentHandler.HandleAsync)
            .WithTags("Appointments");
    }
}





