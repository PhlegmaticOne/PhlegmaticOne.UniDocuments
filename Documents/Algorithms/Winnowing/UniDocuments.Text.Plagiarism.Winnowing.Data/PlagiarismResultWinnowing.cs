using UniDocuments.Text.Features.Fingerprint.Models;
using UniDocuments.Text.Plagiarism.Algorithms.Core;

namespace UniDocuments.Text.Plagiarism.Winnowing.Data;

public class PlagiarismResultWinnowing : IPlagiarismResult
{
    public PlagiarismResultWinnowing(DocumentFingerprint fingerprint, bool isSucceed)
    {
        Fingerprint = fingerprint;
        IsSucceed = isSucceed;
    }

    public bool IsSucceed { get; }
    public DocumentFingerprint Fingerprint { get; }
}