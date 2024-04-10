using UniDocuments.Text.Domain.Services.StreamReading;
using UniDocuments.Text.Features.Fingerprint.Models;

namespace UniDocuments.Text.Features.Fingerprint.Services;

public interface IFingerprintAlgorithm
{
    DocumentFingerprint Fingerprint(StreamContentReadResult text);
}