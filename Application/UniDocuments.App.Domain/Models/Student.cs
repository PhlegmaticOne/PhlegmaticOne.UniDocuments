using UniDocuments.App.Domain.Models.Base;

namespace UniDocuments.App.Domain.Models;

public class Student : Person
{
    public IList<StudyActivity> Activities { get; set; } = null!;
    public IList<StudyDocument> Documents { get; set; } = null!;

    public Student WithPassword(string password)
    {
        return new Student
        {
            Role = Role,
            Password = password,
            FirstName = FirstName,
            LastName = LastName,
            UserName = UserName,
            Id = Id
        };
    }
}
