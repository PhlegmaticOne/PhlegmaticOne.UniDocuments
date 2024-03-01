namespace PhlegmaticOne.UniDocuments.Documents.Features.Metrics.Models;

public class Fingerprint
{
    private readonly HashSet<FingerprintEntry> _fingerprints = new();

    public void AddFingerprint(FingerprintEntry fingerprint)
    {
        _fingerprints.Add(fingerprint);
    }

    public IReadOnlyList<FingerprintEntry> ToList()
    {
        return _fingerprints.ToList();
    }
}