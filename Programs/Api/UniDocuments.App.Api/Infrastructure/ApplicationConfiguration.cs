using PhlegmaticOne.JwtTokensGeneration.Options;

namespace UniDocuments.App.Api.Infrastructure;

public class JwtApplicationSecrets
{
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int ExpirationDurationInMinutes { get; set; }
    public string SecretKey { get; set; } = null!;
}

public class TestConfiguration
{
    public Guid UserId { get; set; }
    public Guid ActivityId { get; set; }
}

public class ApplicationConfiguration
{
    public bool UseAuthentication { get; set; }
    public bool UseRealDatabase { get; set; }
    public string CurrentKerasOptions { get; set; } = null!;
    public JwtApplicationSecrets JwtSecrets { get; set; } = null!;
    public TestConfiguration TestConfiguration { get; set; } = null!;

    public IJwtOptions CreateJwtOptions()
    {
        return new SymmetricKeyJwtOptions(
            JwtSecrets.Issuer,
            JwtSecrets.Audience,
            JwtSecrets.ExpirationDurationInMinutes,
            JwtSecrets.SecretKey);
    }
}