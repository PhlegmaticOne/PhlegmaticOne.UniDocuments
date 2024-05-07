using UniDocuments.Text.Domain.Services.Fingerprinting.Models;
using UniDocuments.Text.Domain.Services.Reports.Models;

namespace UniDocuments.Text.Domain.Services.Fingerprinting;

public interface IFingerprintsComparer
{
    List<ReportDocumentData> Compare(TextFingerprint source, List<TextFingerprint> other);
}