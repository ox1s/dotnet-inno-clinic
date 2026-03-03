using ErrorOr;

using Identity.Application.Authentication.Commands.Register;
using Identity.Application.Authentication.Commands.RevokeRefreshToken;
using Identity.Application.Authentication.Commands.VerifyEmail;
using Identity.Application.Authentication.Common;
using Identity.Application.Authentication.Queries.Login;
using Identity.Application.Authentication.Queries.LoginWithRefreshToken;
using Identity.Contracts.Authentication;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost("patient/register")]
    public Task<IActionResult> RegisterPatient(RegisterRequest request)
    {
        return RegisterByCommand(new RegisterCommand(request.Email, request.Password));
    }

    [HttpPost("doctor/register")]
    public Task<IActionResult> RegisterDoctor(RegisterRequest request)
    {
        return RegisterByCommand(new RegisterCommand(request.Email, request.Password));
    }

    [HttpPost("receptionist/register")]
    public Task<IActionResult> RegisterReceptionist(RegisterRequest request)
    {
        return RegisterByCommand(new RegisterCommand(request.Email, request.Password));
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
        var query = new LoginQuery(request.Email, request.Password, request.HardCodedRole);
        var loginResult = await mediator.Send(query);

        if (loginResult.IsError && loginResult.FirstError == AuthenticationErrors.InvalidCredentials)
            return Problem(
                loginResult.FirstError.Description,
                statusCode: StatusCodes.Status401Unauthorized);

        return loginResult.Match(
            loginResult => Ok(new LoginResponse(
                loginResult.Token,
                loginResult.RefreshToken)),
            Problem);
    }
    [HttpPost("login-with-refresh-token")]
    public async Task<IActionResult> LoginWithRefreshToken(
        [FromBody] LoginWithRefreshTokenRequest request)
    {
        var query = new LoginWithRefreshTokenQuery(request.RefreshToken);
        var loginResult = await mediator.Send(query);

        if (loginResult.IsError
            && loginResult.FirstError == AuthenticationErrors.InvalidRefreshToken)
            return Problem(
                loginResult.FirstError.Description,
                statusCode: StatusCodes.Status401Unauthorized);

        return loginResult.Match(
            loginResult => Ok(new LoginWithRefreshTokenResponse(
                loginResult.Token,
                loginResult.RefreshToken)),
            Problem);
    }
    [HttpDelete("accounts/{id:guid}/refresh-tokens")]
    public async Task<IActionResult> RevokeRefreshTokens(
        Guid id)
    {
        var command = new RevokeRefreshTokensCommand(id);
        var result = await mediator.Send(command);

        return result.Match(
            deleted => Ok("Refresh tokens revoked successfully!"),
            Problem);
    }

    private async Task<IActionResult> RegisterByCommand(IRequest<ErrorOr<AuthenticationResult>> command)
    {
        ErrorOr<AuthenticationResult> authResult = await mediator.Send(command);

        return authResult.Match(
            authResult => base.Ok(MapToAuthResponse(authResult)),
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