using UniDocuments.App.Shared.Users.Enums;

namespace UniDocuments.App.Shared.Users;

public class UpdateProfileObject
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string OldPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
    public AppRole AppRole { get; set; }
}