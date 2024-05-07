using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

namespace UniDocuments.Text.Domain.Providers.Reports.Builder;

public class ReportDataBuildRequest
{
    public ReportDataBuildRequest(UniDocument document, PlagiarismSearchResponseDocument plagiarismSearchResponse, string baseMetric)
    {
        Document = document;
        PlagiarismSearchResponse = plagiarismSearchResponse;
        BaseMetric = baseMetric;
    }

    public UniDocument Document { get; }
    public PlagiarismSearchResponseDocument PlagiarismSearchResponse { get; }
    public string BaseMetric { get; }
}