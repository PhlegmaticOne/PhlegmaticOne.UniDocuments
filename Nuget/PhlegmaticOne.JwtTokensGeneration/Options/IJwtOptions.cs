using Microsoft.IdentityModel.Tokens;

namespace PhlegmaticOne.JwtTokensGeneration.Options;

public interface IJwtOptions
{
    string Issuer { get; }
    string Audience { get; }
    int ExpirationDurationInMinutes { get; }
    SecurityKey GetSecretKey();
}