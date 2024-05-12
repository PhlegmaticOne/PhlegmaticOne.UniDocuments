using Microsoft.AspNetCore.Http;

namespace UniDocuments.App.Shared.Documents;

public class DocumentUploadObject
{
    public Guid Id { get; set; }
    public IFormFile File { get; set; } = null!;
}