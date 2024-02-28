﻿namespace PhlegmaticOne.UniDocuments.Domain.Models.Metrics;

public class Fingerprint
{
    private readonly HashSet<FingerprintEntry> _fingerprints;

    public Fingerprint()
    {
        _fingerprints = new HashSet<FingerprintEntry>();
    }

    public void AddFingerprint(FingerprintEntry fingerprint)
    {
        _fingerprints.Add(fingerprint);
    }

    public IReadOnlyList<FingerprintEntry> ToList()
    {
        return _fingerprints.ToList();
    }
}