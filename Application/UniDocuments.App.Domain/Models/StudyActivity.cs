using UniDocuments.App.Domain.Models.Base;

namespace UniDocuments.App.Domain.Models;

public class StudyActivity : EntityBase
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid CreatorId { get; set; }
    public Teacher Creator { get; set; } = null!;
    public IList<Student> Students { get; set; } = null!;
    public IList<StudyDocument> Documents { get; set; } = null!;

    public StudyActivity ToAnyActivity()
    {
        return new StudyActivity
        {
            Description = Description,
            Name = Name,
            StartDate = DateTime.MinValue,
            EndDate = DateTime.MaxValue,
            Id = Id
        };
    }
}
