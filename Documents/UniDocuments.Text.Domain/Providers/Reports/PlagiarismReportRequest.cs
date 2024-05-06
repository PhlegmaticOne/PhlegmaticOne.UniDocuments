using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;

namespace UniDocuments.Text.Domain.Providers.Reports;

public class PlagiarismReportRequest
{
    public PlagiarismSearchRequest PlagiarismSearchRequest { get; }
    public string BaseMetric { get; }

    public PlagiarismReportRequest(PlagiarismSearchRequest plagiarismSearchRequest, string baseMetric)
    {
        PlagiarismSearchRequest = plagiarismSearchRequest;
        BaseMetric = baseMetric;
    }
}