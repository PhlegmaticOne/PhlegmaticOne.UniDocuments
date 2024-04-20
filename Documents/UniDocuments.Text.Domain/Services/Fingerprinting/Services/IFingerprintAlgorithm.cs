using UniDocuments.Text.Domain.Services.Fingerprinting.Options;

namespace UniDocuments.Text.Domain.Services.Fingerprinting.Services;

public interface IFingerprintAlgorithm
{
    TextFingerprint Fingerprint(UniDocumentContent text, FingerprintOptions options);
}