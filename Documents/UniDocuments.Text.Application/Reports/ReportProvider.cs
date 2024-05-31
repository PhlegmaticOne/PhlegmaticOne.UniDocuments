using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.Reports.Builder;
using UniDocuments.Text.Domain.Providers.Reports.Provider;
using UniDocuments.Text.Domain.Services.Reports;

namespace UniDocuments.Text.Application.Reports;

public class ReportProvider : IReportProvider
{
    private readonly IPlagiarismSearchProvider _plagiarismSearchProvider;
    private readonly IReportDataBuilder _reportDataBuilder;
    private readonly IReportCreator _reportCreator;

    public ReportProvider(
        IPlagiarismSearchProvider plagiarismSearchProvider,
        IReportDataBuilder reportDataBuilder,
        IReportCreator reportCreator)
    {
        _plagiarismSearchProvider = plagiarismSearchProvider;
        _reportDataBuilder = reportDataBuilder;
        _reportCreator = reportCreator;
    }

    public async Task<ReportResponse> BuildReportAsync(ReportRequest request, CancellationToken cancellationToken)
    {
        var p = request.PlagiarismSearchRequest;
        var plagiarismResponse = await _plagiarismSearchProvider.SearchAsync(p);
        var reportDataRequest = new ReportDataBuildRequest(p.Document, plagiarismResponse, request.BaseMetric);
        var reportData = await _reportDataBuilder.BuildReportDataAsync(reportDataRequest, cancellationToken);
        return await _reportCreator.BuildReportAsync(reportData, cancellationToken);
    }
}