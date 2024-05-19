using UniDocuments.Text.Domain.Providers.Reports.Builder;
using UniDocuments.Text.Domain.Services.Reports.Models;

namespace UniDocuments.Text.Domain.Services.Reports;

public interface IReportDataLoader
{
    Task<Dictionary<Guid, ReportLoadData>> LoadAsync(
        ReportDataBuildRequest request, CancellationToken cancellationToken);
}