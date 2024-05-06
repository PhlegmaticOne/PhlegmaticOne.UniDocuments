namespace UniDocuments.Text.Domain.Providers.Reports;

public interface IPlagiarismReportProvider
{
    Task<PlagiarismReport> BuildReportAsync(PlagiarismReportRequest request, CancellationToken cancellationToken);
}