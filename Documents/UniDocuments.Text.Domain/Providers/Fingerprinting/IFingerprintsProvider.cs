using UniDocuments.Text.Domain.Services.Fingerprinting.Models;

namespace UniDocuments.Text.Domain.Providers.Fingerprinting;

public interface IFingerprintsProvider
{
    TextFingerprint Compute(UniDocument document);
    TextFingerprint Get(UniDocument content);
    FingerprintCompareResult Compare(TextFingerprint a, TextFingerprint b);
    Task<Dictionary<Guid, TextFingerprint>> GetForDocumentsAsync(IEnumerable<Guid> documentIds, CancellationToken cancellationToken);
}