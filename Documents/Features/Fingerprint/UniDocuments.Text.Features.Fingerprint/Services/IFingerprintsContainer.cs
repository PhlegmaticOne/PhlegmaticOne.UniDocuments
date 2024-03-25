using UniDocuments.Text.Features.Fingerprint.Models;

namespace UniDocuments.Text.Features.Fingerprint.Services;

public interface IFingerprintsContainer
{
    DocumentFingerprint? Get(Guid documentId);
    void AddOrReplace(Guid documentId, DocumentFingerprint fingerprint);
}