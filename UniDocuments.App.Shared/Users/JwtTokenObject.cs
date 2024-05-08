namespace UniDocuments.App.Shared.Users;

public class JwtTokenObject
{
    public string? Token { get; init; }
    public int ExpirationInMinutes { get; init; }
}