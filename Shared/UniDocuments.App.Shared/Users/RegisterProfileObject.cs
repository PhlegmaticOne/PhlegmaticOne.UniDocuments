namespace UniDocuments.App.Shared.Users;

public class RegisterProfileObject : IdentityBaseObject
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}