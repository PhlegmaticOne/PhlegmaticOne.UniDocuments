using UniDocuments.Text.Domain.Services.Reports;

namespace UniDocuments.Text.Domain.Providers.Reports.Provider;

public interface IReportProvider
{
    Task<ReportResponse> BuildReportAsync(ReportRequest request, CancellationToken cancellationToken);
}