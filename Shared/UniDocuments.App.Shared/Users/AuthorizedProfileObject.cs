namespace UniDocuments.App.Shared.Users;

public class AuthorizedProfileObject
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public JwtTokenObject JwtToken { get; set; } = null!;
}