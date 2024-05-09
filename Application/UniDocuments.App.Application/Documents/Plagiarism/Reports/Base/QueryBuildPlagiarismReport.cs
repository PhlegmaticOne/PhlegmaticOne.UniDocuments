using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.Reports.Provider;
using UniDocuments.Text.Domain.Services.Reports;

namespace UniDocuments.App.Application.Documents.Plagiarism.Reports.Base;

public class QueryBuildPlagiarismReport : IOperationResultQuery<ReportResponse>
{
    public int TopCount { get; set; }
    public int InferEpochs { get; set; }
    public string ModelName { get; set; } = null!;
    public string BaseMetric { get; set; } = null!;

    public ReportRequest ToReportRequest(UniDocument document)
    {
        var plagiarismRequest = new PlagiarismSearchRequest(document, TopCount, InferEpochs, ModelName);
        return new ReportRequest(plagiarismRequest, BaseMetric);
    }
}