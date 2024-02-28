using PhlegmaticOne.UniDocuments.Domain.Models.Metrics;

namespace PhlegmaticOne.UniDocuments.Documents.Core.Features.Metrics;

public class UniDocumentFeatureFingerprint : IUniDocumentFeature
{
    public UniDocumentFeatureFingerprint(Fingerprint fingerprint)
    {
        Fingerprint = fingerprint;
    }

    public UniDocumentFeatureFlag FeatureFlag => UniDocumentFeatureFlag.Fingerprint;
    public Fingerprint Fingerprint { get; }
}
