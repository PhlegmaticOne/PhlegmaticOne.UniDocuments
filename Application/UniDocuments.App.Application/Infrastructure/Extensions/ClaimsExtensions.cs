using System.Security.Claims;
using PhlegmaticOne.JwtTokensGeneration.Helpers;
using UniDocuments.App.Shared.Users.Enums;

namespace UniDocuments.App.Application.Infrastructure.Extensions;

public static class ClaimsExtensions
{
    public static StudyRole StudyRole(this ClaimsPrincipal claimsPrincipal)
    {
        var claimValue = claimsPrincipal.Claims.First(x => x.Type == CustomJwtClaimNames.StudyRole).Value;
        return (StudyRole)int.Parse(claimValue);
    }
}