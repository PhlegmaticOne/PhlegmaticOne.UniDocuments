using UniDocuments.App.Domain.Models.Base;

namespace UniDocuments.App.Domain.Models;

public class Teacher : Person
{
    public IList<StudyActivity> Activities { get; set; } = null!;
}