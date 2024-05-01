namespace UniDocuments.App.Shared.Users;

public class JwtTokenObject
{
    public JwtTokenObject(string token)
    {
        Token = token;
    }

    public string? Token { get; init; }
}