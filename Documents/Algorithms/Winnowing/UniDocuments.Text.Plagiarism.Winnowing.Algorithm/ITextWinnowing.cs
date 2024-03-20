using UniDocuments.Text.Features.Fingerprint.Models;

namespace UniDocuments.Text.Plagiarism.Winnowing.Algorithm;

public interface ITextWinnowing
{
    DocumentFingerprint Winnowing(string text);
}