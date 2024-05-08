using System.Security.Claims;
using UniDocuments.App.Shared.Users.Enums;

namespace UniDocuments.App.Client.Web.Infrastructure.Extensions;

public static class ProfileClaimsConstants
{
    internal const string FirstNameClaimName = "FirstName";
    internal const string LastNameClaimName = "LastName";
    internal const string RoleClaimName = "Role";
}

public static class ClaimPrincipalExtensions
{
    public static StudyRole Role(this ClaimsPrincipal claimsPrincipal)
    {
        var claimValue = claimsPrincipal.Claims
            .FirstOrDefault(x => x.Type == ProfileClaimsConstants.RoleClaimName);

        if (claimValue is null)
        {
            return StudyRole.Student;
        }
        
        return (StudyRole)int.Parse(claimValue.Value);
    }
    
    public static string Username(this ClaimsPrincipal claimsPrincipal)
    {
        var claim = claimsPrincipal.Claims
            .FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType);
        return claim is null ? string.Empty : claim.Value;
    }
    
    public static string Firstname(this ClaimsPrincipal claimsPrincipal)
    {
        var claim = claimsPrincipal.Claims
            .FirstOrDefault(x => x.Type == ProfileClaimsConstants.FirstNameClaimName);
        return claim is null ? string.Empty : claim.Value;
    }

    public static string Lastname(this ClaimsPrincipal claimsPrincipal)
    {
        var claim = claimsPrincipal.Claims
            .FirstOrDefault(x => x.Type == ProfileClaimsConstants.LastNameClaimName);
        return claim is null ? string.Empty : claim.Value;
    }
}