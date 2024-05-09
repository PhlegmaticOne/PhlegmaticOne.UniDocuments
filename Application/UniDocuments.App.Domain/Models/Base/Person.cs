using UniDocuments.App.Domain.Models.Enums;

namespace UniDocuments.App.Domain.Models.Base;

public class Person : EntityBase
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public ApplicationRole Role { get; set; }
}