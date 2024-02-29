using PhlegmaticOne.UniDocuments.Documents.Features.Metrics.Models;

namespace PhlegmaticOne.UniDocuments.Documents.Core.Features.Metrics;

public class UniDocumentFeatureWinnowing : IUniDocumentFeature
{
    public UniDocumentFeatureWinnowing(Fingerprint winnowingFingerprint)
    {
        WinnowingFingerprint = winnowingFingerprint;
    }

    public UniDocumentFeatureFlag FeatureFlag => UniDocumentFeatureFlag.Winnowing;
    public Fingerprint WinnowingFingerprint { get; }
}