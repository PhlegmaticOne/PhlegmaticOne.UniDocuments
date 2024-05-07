namespace UniDocuments.Text.Domain.Services.Fingerprinting;

public interface IFingerprintHash
{
    uint GetHash(string text, int startIndex, int length);
}