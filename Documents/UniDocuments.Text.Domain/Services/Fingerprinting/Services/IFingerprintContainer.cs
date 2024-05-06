namespace UniDocuments.Text.Domain.Services.Fingerprinting.Services;

public interface IFingerprintContainer
{
    TextFingerprint? Get(Guid documentId);
    void AddOrReplace(Guid documentId, TextFingerprint fingerprint);
}