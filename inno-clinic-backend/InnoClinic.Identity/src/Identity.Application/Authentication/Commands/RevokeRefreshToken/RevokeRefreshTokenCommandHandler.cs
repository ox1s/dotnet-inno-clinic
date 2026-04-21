using ErrorOr;

using Identity.Application.Common.Interfaces;

using MediatR;

namespace Identity.Application.Authentication.Commands.RevokeRefreshToken;

public class RevokeRefreshTokensHandler(
    IRefreshTokensRepository refreshTokensRepository,
    IUnitOfWork unitOfWork,
    IUserContext userContext)
    : IRequestHandler<RevokeRefreshTokensCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(
        RevokeRefreshTokensCommand request,
        CancellationToken cancellationToken)
    {
        if (request.AccountId != userContext.UserId)
        {
            return Error.Failure(
                "Authentication.Unauthorized",
                "You are not authorized to revoke these refresh tokens");
        }

        await refreshTokensRepository.RevokeRefreshTokensAsync(request.AccountId,
            cancellationToken);
        await unitOfWork.CommitChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}