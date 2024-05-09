namespace UniDocuments.App.Shared.Users;

public class RegisterObject : LoginObject
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}