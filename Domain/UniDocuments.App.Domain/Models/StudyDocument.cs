using UniDocuments.App.Domain.Models.Base;

namespace UniDocuments.App.Domain.Models;

public class StudyDocument : EntityBase
{
    public DateTime DateLoaded { get; set; }
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public Guid ActivityId { get; set; }
    public StudyActivity Activity { get; set; } = null!;
    public Guid ReportId { get; set; }
    public StudyDocumentReport Report { get; set; } = null!;
    public Guid MetricsId { get; set; }
    public StudyDocumentMetrics Metrics { get; set; } = null!;
}