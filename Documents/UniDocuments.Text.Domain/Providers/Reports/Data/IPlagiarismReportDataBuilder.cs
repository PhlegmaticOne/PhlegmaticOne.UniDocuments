using UniDocuments.Text.Domain.Providers.Reports.Data.Models;

namespace UniDocuments.Text.Domain.Providers.Reports.Data;

public interface IPlagiarismReportDataBuilder
{
    Task<PlagiarismReportData> BuildReportDataAsync(PlagiarismReportDataRequest request, CancellationToken cancellationToken);
}