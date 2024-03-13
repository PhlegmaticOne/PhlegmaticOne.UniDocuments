using UniDocuments.Text.Features.Fingerprint.Models;

namespace UniDocuments.Text.Plagiarism.Winnowing.Algorithm.Algorithm;

public interface ITextWinnowing
{
    DocumentFingerprint Winnowing(string text);
}