using InnoClinic.Shared.DTOs;

using Profile.Infrastructure.Database;
using Profile.Infrastructure.Database.Repositories;

using Wolverine;

namespace Profile.Features.Doctors.EditDoctorStatusByBot;

public class LinkTelegramAccountCommandHandler
{
    public static async Task Handle(
        LinkTelegramAccountCommand command,
        IMessageBus bus)
    {
        await bus.PublishAsync(new TelegramAccountLinked(
            AccountId: command.AccountId,
            TelegramId: command.TelegramId
        ));
    }
}
