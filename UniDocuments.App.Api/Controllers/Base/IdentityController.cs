using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.JwtTokensGeneration.Extensions;
using UniDocuments.App.Application.Infrastructure.Extensions;
using UniDocuments.App.Shared.Users.Enums;

namespace UniDocuments.App.Api.Controllers.Base;

public class IdentityController : ControllerBase
{
    protected Guid ProfileId() => User.GetUserId();
    protected StudyRole StudyRole() => User.StudyRole();
}