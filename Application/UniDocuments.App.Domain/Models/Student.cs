using UniDocuments.App.Domain.Models.Base;

namespace UniDocuments.App.Domain.Models;

public class Student : EntityBase
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public StudyRole Role { get; set; }
    public IList<StudyDocument> Documents { get; set; } = null!;

    public Student WithPassword(string password)
    {
        return new Student
        {
            Password = password,
            FirstName = FirstName,
            LastName = LastName,
            UserName = UserName,
            Id = Id
        };
    }
}
