using Identity.Application.Authentication.Commands.CreateWorkerAccount;
using Identity.Contracts.Authentication;

using InnoClinic.Shared;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[Route("internal/worker-accounts")]
[Authorize(Roles = $"{Roles.Receptionist}")]
public class InternalWorkerAccountsController(ISender mediator) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateWorkerAccountRequest request)
    {
        var command = new CreateWorkerAccountCommand(
            request.Email,
            request.Password,
            request.Role);

        var result = await mediator.Send(command);

        return result.Match(
            created => Ok(new CreateWorkerAccountResponse(
                created.AccountId,
                created.Email,
                created.Role)),
            Problem);
    }
}