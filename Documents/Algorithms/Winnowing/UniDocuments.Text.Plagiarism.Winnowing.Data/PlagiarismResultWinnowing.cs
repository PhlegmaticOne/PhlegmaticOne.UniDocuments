using UniDocuments.Text.Core.Algorithms;

namespace UniDocuments.Text.Plagiarism.Winnowing.Data;

public class PlagiarismResultWinnowing : IPlagiarismResult
{
    public PlagiarismResultWinnowing(Fingerprint fingerprint, bool isSucceed)
    {
        Fingerprint = fingerprint;
        IsSucceed = isSucceed;
    }

    public bool IsSucceed { get; }
    public Fingerprint Fingerprint { get; }
}