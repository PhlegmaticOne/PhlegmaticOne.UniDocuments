using UniDocuments.App.Domain.Models.Base;

namespace UniDocuments.App.Domain.Models;

public class Student : Person
{
    public List<StudyActivity> Activities { get; set; } = new();
    public List<StudyDocument> Documents { get; set; } = new();
}
