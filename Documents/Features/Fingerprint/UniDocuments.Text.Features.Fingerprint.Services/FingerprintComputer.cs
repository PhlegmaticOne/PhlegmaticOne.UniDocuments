using UniDocuments.Text.Domain.Services.StreamReading;
using UniDocuments.Text.Features.Fingerprint.Models;
using UniDocuments.Text.Plagiarism.Winnowing.Algorithm;

namespace UniDocuments.Text.Features.Fingerprint.Services;

public class FingerprintComputer : IFingerprintComputer
{
    private readonly ITextWinnowing _textWinnowing;
    private readonly IFingerprintsContainer _container;

    public FingerprintComputer(ITextWinnowing textWinnowing, IFingerprintsContainer container)
    {
        _textWinnowing = textWinnowing;
        _container = container;
    }
    
    public Task<DocumentFingerprint> ComputeAsync(
        Guid documentId, StreamContentReadResult text, CancellationToken cancellationToken)
    {
        var fingerprint = _textWinnowing.Winnowing(text);
        _container.AddOrReplace(documentId, fingerprint);
        return Task.FromResult(fingerprint);
    }
}