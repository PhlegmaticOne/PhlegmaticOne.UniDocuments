using UniDocuments.Text.Domain.Services.Reports.Models;

namespace UniDocuments.Text.Domain.Services.Reports;

public interface IPlagiarismReportCreator
{
    Task<PlagiarismReport> BuildReportAsync(PlagiarismReportData response, CancellationToken cancellationToken);
}