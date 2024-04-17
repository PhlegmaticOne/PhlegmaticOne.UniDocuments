using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Application.Login.Commands;
using UniDocuments.App.Application.Login.Queries;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterProfileDto registerProfileDto)
    {
        var registerOperationResult =
            await _mediator.Send(new RegisterProfileCommand(registerProfileDto), HttpContext.RequestAborted);

        if (registerOperationResult.IsSuccess == false)
        {
            return BadRequest(registerOperationResult.ErrorMessage);
        }

        return await AuthorizeAsync(registerProfileDto.UserName, registerProfileDto.Password);
    }

    [HttpPost]
    public Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        return AuthorizeAsync(loginDto.UserName, loginDto.Password);
    }

    private async Task<IActionResult> AuthorizeAsync(string email, string password)
    {
        var query = new GetAuthorizedProfileAnonymousQuery(email, password);
        var profile = await _mediator.Send(query, HttpContext.RequestAborted);

        if (profile.IsSuccess == false)
        {
            return BadRequest(profile.ErrorMessage);
        }

        return Ok(profile.Result);
    }
}