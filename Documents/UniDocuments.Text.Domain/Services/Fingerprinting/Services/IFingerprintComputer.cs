namespace UniDocuments.Text.Domain.Services.Fingerprinting.Services;

public interface IFingerprintComputer
{
    Task<TextFingerprint> ComputeAsync(
        Guid documentId, UniDocumentContent text, CancellationToken cancellationToken);
}