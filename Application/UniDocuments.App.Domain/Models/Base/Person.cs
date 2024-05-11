using UniDocuments.App.Domain.Models.Enums;

namespace UniDocuments.App.Domain.Models.Base;

public class Person : EntityBase
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public ApplicationRole Role { get; set; }
    public DateTime JoinDate { get; set; }
    
    public T With<T>(string password, DateTime joinDate) where T : Person, new()
    {
        return new T
        {
            Role = Role,
            Password = password,
            FirstName = FirstName,
            LastName = LastName,
            UserName = UserName,
            JoinDate = joinDate,
            Id = Id
        };
    }
}