using UniDocuments.Text.Domain.Services.Fingerprinting.Models;

namespace UniDocuments.Text.Domain.Providers.Fingerprinting;

public interface IFingerprintsProvider
{
    TextFingerprint Compute(UniDocument document);
    Task<TextFingerprint> ComputeAsync(UniDocument document, CancellationToken cancellationToken);
    Task<TextFingerprint> GetForDocumentAsync(UniDocument content, CancellationToken cancellationToken);
    Task<TextFingerprint> GetForDocumentAsync(Guid documentId, CancellationToken cancellationToken);
    Task<List<TextFingerprint>> GetForDocumentsAsync(IEnumerable<Guid> documentIds, CancellationToken cancellationToken);
}