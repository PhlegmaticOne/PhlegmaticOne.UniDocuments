using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

namespace UniDocuments.Text.Domain.Providers.Reports.Builder;

public class ReportDataBuildRequest
{
    public ReportDataBuildRequest(UniDocument document, PlagiarismSearchResponse plagiarismSearchResponse, string baseMetric)
    {
        Document = document;
        PlagiarismSearchResponse = plagiarismSearchResponse;
        BaseMetric = baseMetric;
    }

    public UniDocument Document { get; }
    public PlagiarismSearchResponse PlagiarismSearchResponse { get; }
    public string BaseMetric { get; }
}