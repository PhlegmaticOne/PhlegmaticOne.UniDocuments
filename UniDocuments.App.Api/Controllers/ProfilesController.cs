using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Api.Controllers.Base;
using UniDocuments.App.Application.Profile;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Api.Controllers;

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
    public async Task<IActionResult> Update([FromBody] UpdateProfileObject updateProfileObject)
    {
        var command = new CommandUpdateProfile(ProfileId(), updateProfileObject);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return Ok(result);
        }

        return Ok(result);
    }
}