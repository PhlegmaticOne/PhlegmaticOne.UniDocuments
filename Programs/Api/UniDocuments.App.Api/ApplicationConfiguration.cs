using PhlegmaticOne.JwtTokensGeneration.Options;

namespace UniDocuments.App.Api;

public class JwtApplicationSecrets
{
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int ExpirationDurationInMinutes { get; set; }
    public string SecretKey { get; set; } = null!;
}

public class ApplicationConfiguration
{
    public bool UseAuthentication { get; set; }
    public bool UseRealDatabase { get; set; }
    public JwtApplicationSecrets JwtSecrets { get; set; } = null!;

    public IJwtOptions CreateJwtOptions()
    {
        return new SymmetricKeyJwtOptions(
            JwtSecrets.Issuer,
            JwtSecrets.Audience,
            JwtSecrets.ExpirationDurationInMinutes,
            JwtSecrets.SecretKey);
    }
}