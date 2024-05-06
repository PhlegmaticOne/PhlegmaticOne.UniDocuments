using FastCache;
using UniDocuments.Text.Domain.Services.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;

namespace UniDocuments.Text.Services.Fingerprinting;

public class FingerprintContainer : IFingerprintContainer
{
    private static readonly TimeSpan CacheTime = TimeSpan.FromMinutes(10);

    public TextFingerprint? Get(Guid documentId)
    {
        return Cached<TextFingerprint>.TryGet(documentId, out var cached) ? cached : null;
    }

    public void AddOrReplace(Guid documentId, TextFingerprint fingerprint)
    {
        Cached<TextFingerprint>.Save(documentId, fingerprint, CacheTime);
    }
}