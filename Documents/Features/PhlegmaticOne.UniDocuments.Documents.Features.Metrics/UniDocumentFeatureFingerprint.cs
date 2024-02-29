using PhlegmaticOne.UniDocuments.Documents.Core.Features;
using PhlegmaticOne.UniDocuments.Documents.Features.Metrics.Models;

namespace PhlegmaticOne.UniDocuments.Documents.Features.Metrics;

public class UniDocumentFeatureFingerprint : IUniDocumentFeature
{
    public UniDocumentFeatureFingerprint(Fingerprint fingerprint)
    {
        Fingerprint = fingerprint;
    }

    public UniDocumentFeatureFlag FeatureFlag => UniDocumentFeatureFlag.Fingerprint;
    public Fingerprint Fingerprint { get; }
}
