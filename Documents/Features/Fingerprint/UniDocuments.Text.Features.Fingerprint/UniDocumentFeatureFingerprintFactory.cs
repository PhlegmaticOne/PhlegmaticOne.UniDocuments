using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Features;
using UniDocuments.Text.Domain.Features.Factories;
using UniDocuments.Text.Features.Fingerprint.Services;

namespace UniDocuments.Text.Features.Fingerprint;

public class UniDocumentFeatureFingerprintFactory : IUniDocumentFeatureFactory
{
    private readonly IFingerprintsContainer _fingerprintsContainer;

    public UniDocumentFeatureFingerprintFactory(IFingerprintsContainer fingerprintsContainer)
    {
        _fingerprintsContainer = fingerprintsContainer;
    }
    
    public UniDocumentFeatureFlag FeatureFlag => UniDocumentFeatureFingerprintFlag.Instance;
    
    public Task<IUniDocumentFeature> CreateFeature(UniDocument document, CancellationToken cancellationToken)
    {
        var fingerprint = _fingerprintsContainer.Get(document.Id);
        IUniDocumentFeature result = new UniDocumentFeatureFingerprint(fingerprint!);
        return Task.FromResult(result);
    }
}