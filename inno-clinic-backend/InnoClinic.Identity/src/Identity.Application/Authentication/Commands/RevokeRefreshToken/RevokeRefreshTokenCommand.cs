using ErrorOr;

using MediatR;
namespace Identity.Application.Authentication.Commands.RevokeRefreshToken;

public record RevokeRefreshTokensCommand(Guid AccountId)
    : IRequest<ErrorOr<Deleted>>;