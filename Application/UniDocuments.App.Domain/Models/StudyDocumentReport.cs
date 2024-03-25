using UniDocuments.App.Domain.Models.Base;

namespace UniDocuments.App.Domain.Models;

public class StudyDocumentReport : EntityBase
{
    public string Description { get; set; } = null!;
    public Guid DocumentId { get; set; }
    public StudyDocument Document { get; set; } = null!;
}
