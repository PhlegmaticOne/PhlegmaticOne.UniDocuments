namespace UniDocuments.App.Api.Controllers.Requests;

public class DocumentCompareFingerprintsRequest
{
    public IFormFile Source { get; set; } = null!;
    public IFormFile Suspicious { get; set; } = null!;
}