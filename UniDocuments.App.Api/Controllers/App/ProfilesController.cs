﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Api.Controllers.Base;
using UniDocuments.App.Api.Infrastructure.Roles;
using UniDocuments.App.Application.App.Profile;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Api.Controllers.App;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfilesController : IdentityController
{
    private readonly IMediator _mediator;

    public ProfilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update(
        [FromBody] UpdateProfileObject updateProfileObject, CancellationToken cancellationToken)
    {
        var command = new CommandUpdateProfile(ProfileId(), StudyRole(), updateProfileObject);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("MakeAdmin")]
    [RequireAppRoles(Shared.Users.Enums.AppRole.Admin)]
    public async Task<IActionResult> MakeAdmin(
        [FromBody] CommandUpdateRole command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}