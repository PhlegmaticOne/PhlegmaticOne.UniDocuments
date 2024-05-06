using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.Reports;
using UniDocuments.Text.Domain.Providers.Reports.Data;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.App.Application.Plagiarism.Reports;

public class QueryBuildPlagiarismDocumentReport : IOperationResultQuery<PlagiarismReport>
{
    public Stream FileStream { get; set; } = null!;
    public int TopN { get; set; }
    public string ModelName { get; set; } = null!;
    public string BaseMetric { get; set; } = null!;

    public PlagiarismSearchRequest ToPlagiarismSearchRequest(UniDocument document)
    {
        return new PlagiarismSearchRequest(document, TopN, ModelName);
    }
}

public class QueryBuildPlagiarismDocumentReportHandler : 
    IOperationResultQueryHandler<QueryBuildPlagiarismDocumentReport, PlagiarismReport>
{
    private readonly IPlagiarismSearchProvider _plagiarismSearchProvider;
    private readonly IPlagiarismReportDataBuilder _reportDataBuilder;
    private readonly IPlagiarismReportProvider _reportProvider;
    private readonly IStreamContentReader _contentReader;

    public QueryBuildPlagiarismDocumentReportHandler(
        IPlagiarismSearchProvider plagiarismSearchProvider,
        IPlagiarismReportDataBuilder reportDataBuilder,
        IPlagiarismReportProvider reportProvider,
        IStreamContentReader contentReader)
    {
        _plagiarismSearchProvider = plagiarismSearchProvider;
        _reportDataBuilder = reportDataBuilder;
        _reportProvider = reportProvider;
        _contentReader = contentReader;
    }
    
    public async Task<OperationResult<PlagiarismReport>> Handle(
        QueryBuildPlagiarismDocumentReport request, CancellationToken cancellationToken)
    {
        var content = await _contentReader.ReadAsync(request.FileStream, cancellationToken);
        var document = UniDocument.FromContent(content);
        
        var plagiarismRequest = request.ToPlagiarismSearchRequest(document);
        var plagiarismResponse = await _plagiarismSearchProvider.SearchAsync(plagiarismRequest, cancellationToken);
        var reportDataRequest = new PlagiarismReportDataRequest(document, plagiarismResponse, request.BaseMetric);
        var reportData = await _reportDataBuilder.BuildReportDataAsync(reportDataRequest, cancellationToken);
        var report = await _reportProvider.BuildReportAsync(reportData, cancellationToken);
        return OperationResult.Successful(report);
    }
}