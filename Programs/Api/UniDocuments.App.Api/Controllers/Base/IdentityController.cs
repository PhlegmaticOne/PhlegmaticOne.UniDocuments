using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.JwtTokensGeneration.Extensions;

namespace UniDocuments.App.Api.Controllers.Base;

public class IdentityController : ControllerBase
{
    protected Guid ProfileId() => User.GetUserId();
}