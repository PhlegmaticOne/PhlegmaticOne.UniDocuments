using System.Security.Claims;
using UniDocuments.App.Client.Web.Infrastructure.Extensions;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Client.Web.Infrastructure.Helpers;

public class ClaimsPrincipalGenerator
{
    public static ClaimsPrincipal GenerateClaimsPrincipal(AuthorizedProfileDto authorizedProfileDto)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, authorizedProfileDto.UserName),
            new(ProfileClaimsConstants.FirstNameClaimName, authorizedProfileDto.FirstName),
            new(ProfileClaimsConstants.LastNameClaimName, authorizedProfileDto.LastName)
        };

        var claimsIdentity = new ClaimsIdentity(claims,
            "ApplicationCookie",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

        return new ClaimsPrincipal(claimsIdentity);
    }
}