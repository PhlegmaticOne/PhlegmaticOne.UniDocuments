using UniDocuments.Text.Algorithms.Hashing;

namespace UniDocuments.Text.Features.Fingerprint.Services;

public class FingerprintHashCrc32C : IFingerprintHash
{
    public uint GetHash(string text, int startIndex, int length)
    {
        return Crc32C.Compute(text, startIndex, length);
    }
}