using UniDocuments.Text.Domain.Services.Reports.Models;

namespace UniDocuments.Text.Domain.Services.Reports;

public interface IReportCreator
{
    Task<ReportResponse> BuildReportAsync(ReportData response, CancellationToken cancellationToken);
}