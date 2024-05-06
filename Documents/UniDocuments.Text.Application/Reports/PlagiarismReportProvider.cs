using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.Reports;
using UniDocuments.Text.Domain.Providers.Reports.Data;

namespace UniDocuments.Text.Application.Reports;

public class PlagiarismReportProvider : IPlagiarismReportProvider
{
    private readonly IPlagiarismSearchProvider _plagiarismSearchProvider;
    private readonly IPlagiarismReportDataBuilder _reportDataBuilder;
    private readonly IPlagiarismReportCreator _reportCreator;

    public PlagiarismReportProvider(
        IPlagiarismSearchProvider plagiarismSearchProvider,
        IPlagiarismReportDataBuilder reportDataBuilder,
        IPlagiarismReportCreator reportCreator)
    {
        _plagiarismSearchProvider = plagiarismSearchProvider;
        _reportDataBuilder = reportDataBuilder;
        _reportCreator = reportCreator;
    }

    public async Task<PlagiarismReport> BuildReportAsync(PlagiarismReportRequest request, CancellationToken cancellationToken)
    {
        var p = request.PlagiarismSearchRequest;
        var plagiarismResponse = await _plagiarismSearchProvider.SearchAsync(p, cancellationToken);
        var reportDataRequest = new PlagiarismReportDataRequest(p.Document, plagiarismResponse, request.BaseMetric);
        var reportData = await _reportDataBuilder.BuildReportDataAsync(reportDataRequest, cancellationToken);
        return await _reportCreator.BuildReportAsync(reportData, cancellationToken);
    }
}