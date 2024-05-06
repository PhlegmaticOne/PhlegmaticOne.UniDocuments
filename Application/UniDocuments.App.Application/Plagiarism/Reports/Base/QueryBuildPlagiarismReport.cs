using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.Reports;

namespace UniDocuments.App.Application.Plagiarism.Reports.Base;

public class QueryBuildPlagiarismReport : IOperationResultQuery<PlagiarismReport>
{
    public int TopCount { get; set; }
    public string ModelName { get; set; } = null!;
    public string BaseMetric { get; set; } = null!;

    public PlagiarismReportRequest ToReportRequest(UniDocument document)
    {
        var plagiarismRequest = new PlagiarismSearchRequest(document, TopCount, ModelName);
        return new PlagiarismReportRequest(plagiarismRequest, BaseMetric);
    }
}