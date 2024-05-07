using UniDocuments.Text.Domain.Services.Fingerprinting.Models;
using UniDocuments.Text.Domain.Services.Fingerprinting.Options;

namespace UniDocuments.Text.Domain.Services.Fingerprinting;

public interface IFingerprintAlgorithm
{
    TextFingerprint Fingerprint(UniDocument document, FingerprintOptions options);
}