using Appointment.Api.Common;
using Appointment.Api.Endpoints;

using InnoClinic.Shared;

using Microsoft.AspNetCore.Http.HttpResults;

namespace Appointment.Api.Features.UpdateAppointment;

public sealed class UpdateAppointmentEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/appointments/{id}", UpdateAppointmentHandler.HandleAsync)
            .WithTags("Appointments")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Patient, Roles.Receptionist));
    }
}