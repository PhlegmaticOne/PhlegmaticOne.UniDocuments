using UniDocuments.Text.Features.Fingerprint.Models;

namespace UniDocuments.Text.Features.Fingerprint.Services;

public class FingerprintsContainer : IFingerprintsContainer
{
    private readonly Dictionary<Guid, DocumentFingerprint?> _fingerprints = new();
    
    public IReadOnlyDictionary<Guid, DocumentFingerprint?> GetAll()
    {
        return _fingerprints;
    }

    public DocumentFingerprint? Get(Guid documentId)
    {
        return _fingerprints.GetValueOrDefault(documentId, null);
    }

    public void AddOrReplace(Guid documentId, DocumentFingerprint fingerprint)
    {
        _fingerprints[documentId] = fingerprint;
    }
}