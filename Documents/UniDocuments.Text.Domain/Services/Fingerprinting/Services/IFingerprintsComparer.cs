using UniDocuments.Text.Domain.Providers.Reports.Data.Models;

namespace UniDocuments.Text.Domain.Services.Fingerprinting.Services;

public interface IFingerprintsComparer
{
    List<ReportDocumentData> Compare(TextFingerprint source, List<TextFingerprint> other);
}