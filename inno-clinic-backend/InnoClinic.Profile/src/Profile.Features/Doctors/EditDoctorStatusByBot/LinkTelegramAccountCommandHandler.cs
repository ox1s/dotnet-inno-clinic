using InnoClinic.Shared.DTOs;

using Wolverine;

namespace Profile.Features.Doctors.EditDoctorStatusByBot;

public static class LinkTelegramAccountCommandHandler
{
    public static async Task HandleAsync(
        LinkTelegramAccountCommand command,
        IMessageBus bus)
    {
        await bus.PublishAsync(new TelegramAccountLinked(
            AccountId: command.AccountId,
            TelegramId: command.TelegramId
        ));
    }
}