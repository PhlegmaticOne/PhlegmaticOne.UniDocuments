using UniDocuments.Text.Domain.Services.Reports.Models;

namespace UniDocuments.Text.Domain.Providers.Reports.Builder;

public interface IReportDataBuilder
{
    Task<PlagiarismReportData> BuildReportDataAsync(ReportDataBuildRequest buildRequest, CancellationToken cancellationToken);
}