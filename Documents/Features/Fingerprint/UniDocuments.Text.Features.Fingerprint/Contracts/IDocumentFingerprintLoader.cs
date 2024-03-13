using UniDocuments.Text.Features.Fingerprint.Models;

namespace UniDocuments.Text.Features.Fingerprint.Contracts;

public interface IDocumentFingerprintLoader
{
    Task<DocumentFingerprint> LoadFingerprintAsync(Guid documentId);
}