using UniDocuments.Text.Domain.Services.Fingerprinting.Models;

namespace UniDocuments.Text.Domain.Services.Fingerprinting;

public interface IFingerprintContainer
{
    TextFingerprint? Get(Guid documentId);
    void AddOrReplace(Guid documentId, TextFingerprint fingerprint);
}