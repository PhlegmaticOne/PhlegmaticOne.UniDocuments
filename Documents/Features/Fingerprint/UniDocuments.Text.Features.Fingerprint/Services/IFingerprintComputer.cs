using UniDocuments.Text.Domain.Services.StreamReading;
using UniDocuments.Text.Features.Fingerprint.Models;

namespace UniDocuments.Text.Features.Fingerprint.Services;

public interface IFingerprintComputer
{
    Task<DocumentFingerprint> ComputeAsync(
        Guid documentId, StreamContentReadResult text, CancellationToken cancellationToken);
}