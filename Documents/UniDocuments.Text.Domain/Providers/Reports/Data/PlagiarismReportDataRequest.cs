using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

namespace UniDocuments.Text.Domain.Providers.Reports.Data;

public class PlagiarismReportDataRequest
{
    public PlagiarismReportDataRequest(UniDocument document, PlagiarismSearchResponseDocument plagiarismSearchResponse, string baseMetric)
    {
        Document = document;
        PlagiarismSearchResponse = plagiarismSearchResponse;
        BaseMetric = baseMetric;
    }

    public UniDocument Document { get; }
    public PlagiarismSearchResponseDocument PlagiarismSearchResponse { get; }
    public string BaseMetric { get; }
}