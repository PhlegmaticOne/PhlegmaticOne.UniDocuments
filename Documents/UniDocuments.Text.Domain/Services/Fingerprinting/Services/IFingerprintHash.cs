namespace UniDocuments.Text.Domain.Services.Fingerprinting.Services;

public interface IFingerprintHash
{
    uint GetHash(string text, int startIndex, int length);
}