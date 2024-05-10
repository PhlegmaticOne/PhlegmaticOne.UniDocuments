namespace UniDocuments.App.Api.Controllers.Requests;

public class DocumentUploadRequest
{
    public IFormFile Document { get; set; } = null!;
    public Guid ActivityId { get; set; }
    public Guid StudentId { get; set; }
}