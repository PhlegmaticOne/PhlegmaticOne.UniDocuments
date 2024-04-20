namespace UniDocuments.Text.Domain.Services.Fingerprinting.Services;

public interface IFingerprintContainer
{
    IReadOnlyDictionary<Guid, TextFingerprint> GetAll();
    TextFingerprint Get(Guid documentId);
    void AddOrReplace(Guid documentId, TextFingerprint fingerprint);
}