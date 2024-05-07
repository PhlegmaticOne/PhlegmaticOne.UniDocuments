namespace UniDocuments.App.Api.Infrastructure.Configurations;

public class ApplicationJwtSecrets
{
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int ExpirationDurationInMinutes { get; set; }
    public string SecretKey { get; set; } = null!;
}