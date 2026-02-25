using Appointment.Api.Endpoints;

using InnoClinic.Shared;

namespace Appointment.Api.Features.Doctor.GetSchedule;

public record GetScheduleEndpoint : IEndpoint
{

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/doctor/schedule", GetScheduleHandler.Handle)
            .WithTags("Doctor")
            .WithName("GetDoctorSchedule")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Doctor))
            .WithDescription("US-10 View appointment schedule by doctor");
    }

}
