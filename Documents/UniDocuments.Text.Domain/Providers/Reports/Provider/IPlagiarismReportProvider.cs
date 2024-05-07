using UniDocuments.Text.Domain.Services.Reports;

namespace UniDocuments.Text.Domain.Providers.Reports.Provider;

public interface IPlagiarismReportProvider
{
    Task<PlagiarismReport> BuildReportAsync(PlagiarismReportRequest request, CancellationToken cancellationToken);
}