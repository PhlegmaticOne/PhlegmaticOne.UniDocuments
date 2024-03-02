using UniDocuments.App.Domain.Models.Base;

namespace UniDocuments.App.Domain.Models;

public class Group : EntityBase
{
    public string Name { get; set; } = null!;
    public IList<Student> Students { get; set; } = null!;
    public IList<StudyActivity> Activities { get; set; } = null!;
}
