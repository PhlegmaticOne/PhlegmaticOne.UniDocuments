using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.JwtTokensGeneration.Extensions;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Application.Infrastructure.Extensions;
using UniDocuments.App.Shared.Users.Enums;

namespace UniDocuments.App.Api.Controllers.Base;

public class IdentityController : ControllerBase
{
    protected Guid ProfileId() => User.GetUserId();
    protected string Firstname() => User.Firstname();
    protected string Lastname() => User.Lastname();
    protected StudyRole StudyRole() => User.StudyRole();

    protected IdentityProfileData ProfileData()
    {
        return new IdentityProfileData
        {
            FirstName = Firstname(),
            LastName = Lastname(),
            Id = ProfileId()
        };
    }
}