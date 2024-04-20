using UniDocuments.App.Domain.Models.Base;

namespace UniDocuments.App.Domain.Models;

public class StudyDocument : EntityBase
{
    public DateTime DateLoaded { get; set; }
    public string Name { get; set; } = null!;
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public Guid ActivityId { get; set; }
    public StudyActivity Activity { get; set; } = null!;
    public byte[] WinnowingData { get; set; } = null!;
}