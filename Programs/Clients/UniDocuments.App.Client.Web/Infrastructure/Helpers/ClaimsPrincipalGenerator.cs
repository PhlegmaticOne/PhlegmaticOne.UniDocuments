using System.Security.Claims;
using UniDocuments.App.Client.Web.Infrastructure.Extensions;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Client.Web.Infrastructure.Helpers;

public class ClaimsPrincipalGenerator
{
    public static ClaimsPrincipal GenerateClaimsPrincipal(ProfileObject profileObject)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, profileObject.UserName),
            new(ProfileClaimsConstants.FirstNameClaimName, profileObject.FirstName),
            new(ProfileClaimsConstants.LastNameClaimName, profileObject.LastName),
            new(ProfileClaimsConstants.RoleClaimName, ((int)profileObject.Role).ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims,
            "ApplicationCookie",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

        return new ClaimsPrincipal(claimsIdentity);
    }
}