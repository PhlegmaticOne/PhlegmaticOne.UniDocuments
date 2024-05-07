using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;

namespace UniDocuments.Text.Domain.Providers.Reports.Provider;

public class ReportRequest
{
    public PlagiarismSearchRequest PlagiarismSearchRequest { get; }
    public string BaseMetric { get; }

    public ReportRequest(PlagiarismSearchRequest plagiarismSearchRequest, string baseMetric)
    {
        PlagiarismSearchRequest = plagiarismSearchRequest;
        BaseMetric = baseMetric;
    }
}