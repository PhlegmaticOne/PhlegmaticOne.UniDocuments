using UniDocuments.Text.Domain.Services.Fingerprinting.Services;

namespace UniDocuments.Text.Services.Fingerprinting.Hashing;

public class FingerprintHashCrc32C : IFingerprintHash
{
    public uint GetHash(string text, int startIndex, int length)
    {
        return Crc32C.Compute(text, startIndex, length);
    }
}