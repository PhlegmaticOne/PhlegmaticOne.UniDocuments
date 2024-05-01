using UniDocuments.Text.Domain.Services.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;

namespace UniDocuments.Text.Services.Fingerprinting;

public class FingerprintContainer : IFingerprintContainer
{
    private readonly Dictionary<Guid, TextFingerprint> _fingerprints;

    public FingerprintContainer()
    {
        _fingerprints = new Dictionary<Guid, TextFingerprint>();
    }

    public IReadOnlyDictionary<Guid, TextFingerprint> GetAll()
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