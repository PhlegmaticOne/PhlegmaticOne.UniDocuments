namespace UniDocuments.App.Shared.Users;

public class JwtTokenDto
{
    public JwtTokenDto(string token)
    {
        Token = token;
    }

    public string? Token { get; init; }
}