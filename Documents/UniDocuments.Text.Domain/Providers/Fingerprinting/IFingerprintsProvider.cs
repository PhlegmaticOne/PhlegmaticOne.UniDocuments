using UniDocuments.Text.Domain.Services.Fingerprinting.Models;

namespace UniDocuments.Text.Domain.Providers.Fingerprinting;

public interface IFingerprintsProvider
{
    TextFingerprint Compute(UniDocument document);
    TextFingerprint Get(UniDocument content);
    Task<Dictionary<Guid, TextFingerprint>> GetForDocumentsAsync(IEnumerable<Guid> documentIds, CancellationToken cancellationToken);
}