using Microsoft.EntityFrameworkCore;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.Text.Domain.Services.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;

namespace UniDocuments.Text.Services.Fingerprinting;

public class FingerprintContainer : IFingerprintContainer
{
    private Dictionary<Guid, TextFingerprint> _fingerprints;

    public FingerprintContainer()
    {
        _fingerprints = new Dictionary<Guid, TextFingerprint>();
    }

    public IReadOnlyDictionary<Guid, TextFingerprint> GetAll()
    {
        return _fingerprints;
    }

    public TextFingerprint Get(Guid documentId)
    {
        return _fingerprints[documentId];
    }

    public void AddOrReplace(Guid documentId, TextFingerprint fingerprint)
    {
        _fingerprints[documentId] = fingerprint;
    }
}