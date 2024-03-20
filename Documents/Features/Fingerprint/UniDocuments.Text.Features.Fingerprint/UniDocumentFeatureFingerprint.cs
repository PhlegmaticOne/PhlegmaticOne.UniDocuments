using UniDocuments.Text.Domain.Features;
using UniDocuments.Text.Features.Fingerprint.Models;

namespace UniDocuments.Text.Features.Fingerprint;

public class UniDocumentFeatureFingerprint : IUniDocumentFeature
{
    public DocumentFingerprint Fingerprint { get; }
    public UniDocumentFeatureFlag FeatureFlag => UniDocumentFeatureFingerprintFlag.Instance;

    public UniDocumentFeatureFingerprint(DocumentFingerprint fingerprint)
    {
        Fingerprint = fingerprint;
    }
}