using UniDocuments.App.Domain.Models.Base;

namespace UniDocuments.App.Domain.Models;

public class Teacher : Person
{
    public List<StudyActivity> Activities { get; set; } = new();
}