using UniDocuments.App.Domain.Models.Base;

namespace UniDocuments.App.Domain.Models;

public class StudyDocumentMetrics : EntityBase
{
    public string WinnowingData { get; set; } = null!;
    public StudyDocument Document { get; set; } = null!;
}
