using Appointment.Api.Endpoints;

using InnoClinic.Shared;

namespace Appointment.Api.Features.CreateAppointment;

public sealed class CreatePatientAppointmentEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("patient/appointments", CreatePatientAppointmentHandler.HandleAsync)
            .WithTags("Patient")
            .WithName("CreatePatientAppointment")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Patient))
            .WithDescription("US-6 Create an appointment");
    }
}