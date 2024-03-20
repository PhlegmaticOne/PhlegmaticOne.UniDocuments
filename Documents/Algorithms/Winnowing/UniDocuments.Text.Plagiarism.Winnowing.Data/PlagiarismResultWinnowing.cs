using UniDocuments.Text.Domain.Algorithms;
using UniDocuments.Text.Features.Fingerprint.Models;

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