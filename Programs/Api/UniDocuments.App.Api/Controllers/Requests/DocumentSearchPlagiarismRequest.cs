namespace UniDocuments.App.Api.Controllers.Requests;

public class DocumentSearchPlagiarismRequest
{
    public IFormFile File { get; set; } = null!;
    public int TopCount { get; set; }
    public string ModelName { get; set; } = null!;
}