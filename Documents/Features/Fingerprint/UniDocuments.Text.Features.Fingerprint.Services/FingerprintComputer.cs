using UniDocuments.Text.Domain.Services.StreamReading;
using UniDocuments.Text.Features.Fingerprint.Models;

namespace UniDocuments.Text.Features.Fingerprint.Services;

public class FingerprintComputer : IFingerprintComputer
{
    private readonly IFingerprintAlgorithm _algorithm;
    private readonly IFingerprintsContainer _container;

    public FingerprintComputer(IFingerprintAlgorithm algorithm, IFingerprintsContainer container)
    {
        _algorithm = algorithm;
        _container = container;
    }
    
    public Task<DocumentFingerprint> ComputeAsync(
        Guid documentId, StreamContentReadResult text, CancellationToken cancellationToken)
    {
        var fingerprint = _algorithm.Fingerprint(text);
        _container.AddOrReplace(documentId, fingerprint);
        return Task.FromResult(fingerprint);
    }
}