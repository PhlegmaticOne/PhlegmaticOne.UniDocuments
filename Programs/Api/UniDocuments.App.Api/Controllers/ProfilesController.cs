using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Api.Controllers.Base;
using UniDocuments.App.Application.Profile;

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

    [HttpGet("GetAuthorized")]
    public async Task<IActionResult> GetAuthorized(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new QueryGetDetailedProfile(ProfileId()), cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    // [HttpPut]
    // public async Task<IActionResult> Update(
    //     [FromBody] UpdateProfileObject updateProfileObject, CancellationToken cancellationToken)
    // {
    //     var updateResult = await _mediator.Send(new UpdateProfileCommand(ProfileId(), updateProfileObject),
    //         cancellationToken);
    //
    //     if (updateResult.IsSuccess == false)
    //     {
    //         return OperationResult.Failed<AuthorizedProfileDto>(updateResult.ErrorMessage);
    //     }
    //
    //     return await _mediator.Send(new GetAuthorizedProfileAuthorizedQuery(ProfileId()),
    //         HttpContext.RequestAborted);
    // }
}