namespace UniDocuments.Text.Plagiarism.Winnowing.Algorithm.Hash;

public interface IFingerprintHash
{
    uint GetHash(string text, int startIndex, int length);
}