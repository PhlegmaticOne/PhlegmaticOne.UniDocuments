namespace UniDocuments.Text.Domain.Services.Fingerprinting.Services;

public interface IFingerprintsContainer
{
    IReadOnlyDictionary<Guid, TextFingerprint?> GetAll();
    TextFingerprint? Get(Guid documentId);
    void AddOrReplace(Guid documentId, TextFingerprint fingerprint);
}