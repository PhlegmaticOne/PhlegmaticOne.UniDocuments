namespace UniDocuments.App.Shared.Users;

public class JwtTokenObject
{
    public static JwtTokenObject Empty => new()
    {
        Token = string.Empty,
        ExpirationInMinutes = 0
    };
    
    public string? Token { get; init; }
    public int ExpirationInMinutes { get; init; }
}