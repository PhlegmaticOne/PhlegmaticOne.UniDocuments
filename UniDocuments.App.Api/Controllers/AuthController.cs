using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Application.Login.Commands;
using UniDocuments.App.Application.Login.Queries;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterObject registerObject, CancellationToken cancellationToken)
    {
        var registerCommand = new CommandRegisterProfile(registerObject);
        var registerResult = await _mediator.Send(registerCommand, cancellationToken);

        if (registerResult.IsSuccess == false)
        {
            return BadRequest(registerResult);
        }

        return await AuthorizeAsync(registerObject.UserName, registerObject.Password, cancellationToken);
    }

    [HttpPost("Login")]
    public Task<IActionResult> Login(
        [FromBody] LoginObject loginObject, CancellationToken cancellationToken)
    {
        return AuthorizeAsync(loginObject.UserName, loginObject.Password, cancellationToken);
    }

    private async Task<IActionResult> AuthorizeAsync(string email, string password, CancellationToken cancellationToken)
    {
        var query = new QueryGetAuthorizedProfileAnonymous(email, password);
        var profile = await _mediator.Send(query, cancellationToken);

        if (profile.IsSuccess == false)
        {
            return BadRequest(profile);
        }

        return Ok(profile);
    }
}