namespace UniDocuments.App.Api.Controllers.Requests;

public class DocumentBuildReportRequest
{
    public IFormFile File { get; set; } = null!;
    public int TopCount { get; set; }
    public int InferEpochs { get; set; }
    public string ModelName { get; set; } = null!;
    public string BaseMetric { get; set; } = null!;
}