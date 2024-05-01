using System.Security.Claims;
using UniDocuments.App.Client.Web.Infrastructure.Extensions;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Client.Web.Infrastructure.Helpers;

public class ClaimsPrincipalGenerator
{
    public static ClaimsPrincipal GenerateClaimsPrincipal(AuthorizedProfileObject authorizedProfileObject)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, authorizedProfileObject.UserName),
            new(ProfileClaimsConstants.FirstNameClaimName, authorizedProfileObject.FirstName),
            new(ProfileClaimsConstants.LastNameClaimName, authorizedProfileObject.LastName)
        };

        var claimsIdentity = new ClaimsIdentity(claims,
            "ApplicationCookie",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

        return new ClaimsPrincipal(claimsIdentity);
    }
}