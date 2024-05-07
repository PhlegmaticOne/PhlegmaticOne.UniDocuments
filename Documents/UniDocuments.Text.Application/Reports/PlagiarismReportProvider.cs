using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.Reports;
using UniDocuments.Text.Domain.Providers.Reports.Builder;
using UniDocuments.Text.Domain.Providers.Reports.Provider;
using UniDocuments.Text.Domain.Services.Reports;

namespace UniDocuments.Text.Application.Reports;

public class PlagiarismReportProvider : IPlagiarismReportProvider
{
    private readonly IPlagiarismSearchProvider _plagiarismSearchProvider;
    private readonly IReportDataBuilder _reportDataBuilder;
    private readonly IPlagiarismReportCreator _reportCreator;

    public PlagiarismReportProvider(
        IPlagiarismSearchProvider plagiarismSearchProvider,
        IReportDataBuilder reportDataBuilder,
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
        var reportDataRequest = new ReportDataBuildRequest(p.Document, plagiarismResponse, request.BaseMetric);
        var reportData = await _reportDataBuilder.BuildReportDataAsync(reportDataRequest, cancellationToken);
        return await _reportCreator.BuildReportAsync(reportData, cancellationToken);
    }
}