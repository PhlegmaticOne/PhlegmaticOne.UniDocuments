using PhlegmaticOne.UniDocuments.Domain.Models.Base;

namespace PhlegmaticOne.UniDocuments.Domain.Models;

public class StudyDocumentReport : EntityBase
{
    public string Description { get; set; } = null!;
    public StudyDocument Document { get; set; } = null!;
}
