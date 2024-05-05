using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Services.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Options;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;

namespace UniDocuments.Text.Services.Fingerprinting;

public class FingerprintComputer : IFingerprintComputer
{
    private readonly IFingerprintAlgorithm _algorithm;
    private readonly IFingerprintOptionsProvider _optionsProvider;
    private readonly IFingerprintContainer _container;

    public FingerprintComputer(
        IFingerprintAlgorithm algorithm, 
        IFingerprintOptionsProvider optionsProvider, 
        IFingerprintContainer container)
    {
        _algorithm = algorithm;
        _optionsProvider = optionsProvider;
        _container = container;
    }
    
    public Task<TextFingerprint> ComputeAsync(
        Guid documentId, UniDocumentContent text, CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            var options = _optionsProvider.GetOptions();
            var fingerprint = _algorithm.Fingerprint(text, options);
            _container.AddOrReplace(documentId, fingerprint);
            return Task.FromResult(fingerprint);
        }, cancellationToken);
    }
}