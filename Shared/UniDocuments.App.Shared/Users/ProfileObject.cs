using UniDocuments.App.Shared.Users.Enums;

namespace UniDocuments.App.Shared.Users;

public class ProfileObject
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public StudyRole Role { get; set; }
    public JwtTokenObject JwtToken { get; set; } = null!;
}