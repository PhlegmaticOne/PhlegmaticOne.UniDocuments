using UniDocuments.Text.Domain.Providers.Reports.Data.Models;

namespace UniDocuments.Text.Domain.Providers.Reports;

public interface IPlagiarismReportProvider
{
    Task<PlagiarismReport> BuildReportAsync(PlagiarismReportData response, CancellationToken cancellationToken);
}