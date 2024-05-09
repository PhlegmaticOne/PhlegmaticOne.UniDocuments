using UniDocuments.App.Shared.Users.Enums;

namespace UniDocuments.App.Shared.Users;

public class LoginObject
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public StudyRole StudyRole { get; set; }
}