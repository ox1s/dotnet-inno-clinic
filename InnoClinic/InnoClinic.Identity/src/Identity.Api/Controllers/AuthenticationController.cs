using ErrorOr;
using Identity.Application.Authentication.Commands.Register;
using Identity.Application.Authentication.Commands.VerifyEmail;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Identity.Application.Authentication.Common;
using Identity.Contracts.Authentication;
using Identity.Application.Authentication.Queries.Login;

namespace Identity.Api.Controllers;

[Route("[controller]")]
[AllowAnonymous]
public class AuthenticationController(ISender mediator)
    : ApiController
{

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(request.Email, request.Password);
        ErrorOr<AuthenticationResult> authResult = await mediator.Send(command);

        return authResult.Match(
            authResult => base.Ok(MapToAuthResponse(authResult)),
            Problem);
    }

    [HttpGet("verify-email", Name = "VerifyEmailRoute")]
    public async Task<IActionResult> VerifyEmail([FromQuery] Guid accountId, [FromQuery] string token)
    {
        var command = new VerifyEmailCommand(accountId, token);
        var result = await mediator.Send(command);

        return result.Match(
            success => Ok("Email successfully verified!"),
            Problem);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = new LoginQuery(request.Email, request.Password);
        var authResult = await mediator.Send(query);

        if (authResult.IsError && authResult.FirstError == AuthenticationErrors.InvalidCredentials)
            return Problem(
                authResult.FirstError.Description,
                statusCode: StatusCodes.Status401Unauthorized);

        return authResult.Match(
            authenticationResult => Ok(MapToAuthResponse(authenticationResult)),
            Problem);
    }

    private static AuthenticationResponse MapToAuthResponse(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(
            authResult.Account.Id,
            authResult.Account.Email.Value,
            authResult.Token);
    }

}
