using PhlegmaticOne.UniDocuments.Domain.Models.Base;

namespace PhlegmaticOne.UniDocuments.Domain.Models;

public class StudyActivity : EntityBase
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public IList<Group> Groups { get; set; } = null!;
    public IList<StudyDocument> Documents { get; set; } = null!;
}
