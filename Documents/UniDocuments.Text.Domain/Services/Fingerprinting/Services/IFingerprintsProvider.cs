namespace UniDocuments.Text.Domain.Services.Fingerprinting.Services;

public interface IFingerprintsProvider
{
    Task<TextFingerprint> ComputeAsync(UniDocument document, CancellationToken cancellationToken);
    Task<TextFingerprint> GetForDocumentAsync(UniDocument content, CancellationToken cancellationToken);
    Task<TextFingerprint> GetForDocumentAsync(Guid documentId, CancellationToken cancellationToken);
    Task<List<TextFingerprint>> GetForDocumentsAsync(IEnumerable<Guid> documentIds, CancellationToken cancellationToken);
}