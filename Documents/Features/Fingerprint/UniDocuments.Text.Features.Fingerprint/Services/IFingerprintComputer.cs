using UniDocuments.Text.Features.Fingerprint.Models;

namespace UniDocuments.Text.Features.Fingerprint.Services;

public interface IFingerprintComputer
{
    Task<DocumentFingerprint> ComputeAsync(Guid documentId, string text, CancellationToken cancellationToken);
}