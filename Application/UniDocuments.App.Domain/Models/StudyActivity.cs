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
    public List<Student> Students { get; set; } = new();
    public List<StudyDocument> Documents { get; set; } = new();

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
