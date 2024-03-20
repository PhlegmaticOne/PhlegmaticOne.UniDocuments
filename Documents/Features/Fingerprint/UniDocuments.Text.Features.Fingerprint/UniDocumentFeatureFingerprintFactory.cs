using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Features;
using UniDocuments.Text.Domain.Features.Factories;
using UniDocuments.Text.Features.Fingerprint.Contracts;

namespace UniDocuments.Text.Features.Fingerprint;

public class UniDocumentFeatureFingerprintFactory : IUniDocumentFeatureFactory
{
    private readonly IDocumentFingerprintLoader _fingerprintLoader;

    public UniDocumentFeatureFingerprintFactory(IDocumentFingerprintLoader fingerprintLoader)
    {
        _fingerprintLoader = fingerprintLoader;
    }
    
    public UniDocumentFeatureFlag FeatureFlag => UniDocumentFeatureFingerprintFlag.Instance;
    
    public async Task<IUniDocumentFeature> CreateFeature(UniDocument document)
    {
        var fingerprint = await _fingerprintLoader.LoadFingerprintAsync(document.Id);
        return new UniDocumentFeatureFingerprint(fingerprint);
    }
}