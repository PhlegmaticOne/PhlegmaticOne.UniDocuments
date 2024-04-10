namespace UniDocuments.Text.Features.Fingerprint.Services;

public interface IFingerprintHash
{
    uint GetHash(string text, int startIndex, int length);
}