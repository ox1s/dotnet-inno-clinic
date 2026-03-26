using InnoClinic.Shared.DTOs;

using Profile.Api.Common;

using Profile.Features.Doctors.EditDoctorStatusByBot;

using Wolverine;

namespace Profile.Api.Endpoints;

public sealed class BotEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/bot/doctors/status", async (EditDoctorStatusByBotCommand command, IMessageBus bus) =>
                await bus.InvokeAsync(command))
            .RequireAuthorization("BotPolicy");

        app.MapPost("/bot/accounts/link-telegram", async (LinkTelegramAccountCommand command, IMessageBus bus) =>
                await bus.InvokeAsync(command))
            .RequireAuthorization("BotPolicy");
    }
}
