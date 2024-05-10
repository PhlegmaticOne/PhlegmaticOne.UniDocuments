using UniDocuments.App.Domain.Models.Base;

namespace UniDocuments.App.Domain.Models;

public class Student : Person
{
    public List<StudyActivity> Activities { get; set; } = new();
    public List<StudyDocument> Documents { get; set; } = new();

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
