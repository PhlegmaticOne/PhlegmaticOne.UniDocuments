using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Application.App.Login.Commands;
using UniDocuments.App.Application.App.Login.Queries;
using UniDocuments.App.Shared.Users;
using UniDocuments.App.Shared.Users.Enums;

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

        return Ok(registerResult);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginObject loginObject, CancellationToken cancellationToken)
    {
        var query = new QueryLogin(loginObject);
        var profile = await _mediator.Send(query, cancellationToken);

        if (profile.IsSuccess == false)
        {
            return BadRequest(profile);
        }

        return Ok(profile);
    }
}