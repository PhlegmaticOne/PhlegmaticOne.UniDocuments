using UniDocuments.App.Domain.Models.Base;

namespace UniDocuments.App.Domain.Models;

public class StudyDocumentFile : EntityBase
{
    public StudyDocument StudyDocument { get; set; } = null!;
    public byte[] Content { get; set; } = null!;
    public string Name { get; set; } = null!;
}