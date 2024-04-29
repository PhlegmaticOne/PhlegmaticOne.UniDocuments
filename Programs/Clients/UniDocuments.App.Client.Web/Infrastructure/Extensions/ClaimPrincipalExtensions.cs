using System.Security.Claims;

namespace UniDocuments.App.Client.Web.Infrastructure.Extensions;

public static class ProfileClaimsConstants
{
    internal const string FirstNameClaimName = "FirstName";
    internal const string LastNameClaimName = "LastName";
}

public static class ClaimPrincipalExtensions
{
    public static string Username(this ClaimsPrincipal claimsPrincipal)
    {
        var claimValue = claimsPrincipal.Claims
            .FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType);
        return claimValue is null ? string.Empty : claimValue.Value;
    }
    
    public static string Firstname(this ClaimsPrincipal claimsPrincipal)
    {
        var claimValue = claimsPrincipal.Claims
            .FirstOrDefault(x => x.Type == ProfileClaimsConstants.FirstNameClaimName);
        return claimValue is null ? string.Empty : claimValue.Value;
    }

    public static string Lastname(this ClaimsPrincipal claimsPrincipal)
    {
        var claimValue = claimsPrincipal.Claims
            .FirstOrDefault(x => x.Type == ProfileClaimsConstants.LastNameClaimName);
        return claimValue is null ? string.Empty : claimValue.Value;
    }
}