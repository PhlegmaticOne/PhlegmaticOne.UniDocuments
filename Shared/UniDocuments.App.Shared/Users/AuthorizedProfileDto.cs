namespace UniDocuments.App.Shared.Users;

public class AuthorizedProfileDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public JwtTokenDto JwtToken { get; set; } = null!;
}