using UniDocuments.Text.Domain.Services.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;

namespace UniDocuments.Text.Services.Fingerprinting;

public class FingerprintsContainer : IFingerprintsContainer
{
    private readonly Dictionary<Guid, TextFingerprint?> _fingerprints = new();
    
    public IReadOnlyDictionary<Guid, TextFingerprint?> GetAll()
    {
        return _fingerprints;
    }

    public TextFingerprint? Get(Guid documentId)
    {
        return _fingerprints.GetValueOrDefault(documentId, null);
    }

    public void AddOrReplace(Guid documentId, TextFingerprint fingerprint)
    {
        _fingerprints[documentId] = fingerprint;
    }
}