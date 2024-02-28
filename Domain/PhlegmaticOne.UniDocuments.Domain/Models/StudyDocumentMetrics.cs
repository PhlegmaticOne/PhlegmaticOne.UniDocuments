using PhlegmaticOne.UniDocuments.Domain.Models.Base;

namespace PhlegmaticOne.UniDocuments.Domain.Models;

public class StudyDocumentMetrics : EntityBase
{
    public string WinnowingData { get; set; } = null!;
    public string FingerprintsData { get; set; } = null!;
    public StudyDocument Document { get; set; } = null!;
}
