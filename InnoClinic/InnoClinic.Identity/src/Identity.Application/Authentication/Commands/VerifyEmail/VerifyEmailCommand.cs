using ErrorOr;
using MediatR;

namespace Identity.Application.Authentication.Commands.VerifyEmail;

public record VerifyEmailCommand(Guid AccountId, string Token)
    : IRequest<ErrorOr<Success>>;
