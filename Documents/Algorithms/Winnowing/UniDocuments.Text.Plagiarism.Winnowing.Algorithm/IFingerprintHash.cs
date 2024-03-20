namespace UniDocuments.Text.Plagiarism.Winnowing.Algorithm;

public interface IFingerprintHash
{
    uint GetHash(string text, int startIndex, int length);
}