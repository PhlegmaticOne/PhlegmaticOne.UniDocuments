using PhlegmaticOne.JwtTokensGeneration.Helpers;
using System.Security.Claims;

namespace PhlegmaticOne.JwtTokensGeneration.Extensions;

public static class ClaimsPrincipalExtensions
{
    private const string GivenName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";
    private const string Surname = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";
    
    public static string Firstname(this ClaimsPrincipal claimsPrincipal)
    {
        var claimValue = claimsPrincipal.Claims.First(x => x.Type == GivenName).Value;
        return claimValue;
    }
    
    public static string Lastname(this ClaimsPrincipal claimsPrincipal)
    {
        var claimValue = claimsPrincipal.Claims.First(x => x.Type == Surname).Value;
        return claimValue;
    }
    
    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var claimValue = claimsPrincipal.Claims.First(x => x.Type == CustomJwtClaimNames.UserId).Value;
        return Guid.Parse(claimValue);
    }
}