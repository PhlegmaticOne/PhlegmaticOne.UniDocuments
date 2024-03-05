using UniDocuments.Text.Plagiarism.Winnowing.Data;

namespace UniDocuments.Text.Plagiarism.Winnowing.Algorithm.Algorithm;

public interface ITextWinnowing
{
    Fingerprint Winnowing(string text);
}