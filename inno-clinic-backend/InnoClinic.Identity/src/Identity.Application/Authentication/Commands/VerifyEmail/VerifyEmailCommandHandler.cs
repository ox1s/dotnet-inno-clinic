using ErrorOr;

using Identity.Application.Common.Interfaces;
using Identity.Domain.AccountAggregate;
using Identity.Domain.Common.Interfaces;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Identity.Application.Authentication.Commands.VerifyEmail;

public class VerifyEmailCommandHandler(
    IAccountsRepository accountsRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    ILogger<VerifyEmailCommandHandler> logger)
    : IRequestHandler<VerifyEmailCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var account = await accountsRepository.GetByIdAsync(request.AccountId, cancellationToken);

        if (account is null) return AccountErrors.AccountNotFound;

        var result = account.VerifyEmail(
            request.Token,
            dateTimeProvider);
        if (result.IsError) return result.Errors;

        await accountsRepository.UpdateAsync(account, cancellationToken);
        await unitOfWork.CommitChangesAsync(cancellationToken);

        logger.LogInformation("Verified email for account {AccountId}", request.AccountId);

        return Result.Success;
    }
}