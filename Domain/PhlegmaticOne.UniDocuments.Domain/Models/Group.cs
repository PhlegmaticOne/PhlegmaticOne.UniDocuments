using PhlegmaticOne.UniDocuments.Domain.Models.Base;

namespace PhlegmaticOne.UniDocuments.Domain.Models;

public class Group : EntityBase
{
    public string Name { get; set; } = null!;
    public IList<Student> Students { get; set; } = null!;
    public IList<StudyActivity> Activities { get; set; } = null!;
}
