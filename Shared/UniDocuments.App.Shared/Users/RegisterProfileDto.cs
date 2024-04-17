namespace UniDocuments.App.Shared.Users;

public class RegisterProfileDto : IdentityDtoBase
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}