namespace UniDocuments.Text.Domain.Services.Fingerprinting.Models;

public struct FingerprintCompareResult
{
    public FingerprintCompareResult(double similarity, bool isSuspicious)
    {
        Similarity = similarity;
        IsSuspicious = isSuspicious;
    }

    public double Similarity { get; }
    public bool IsSuspicious { get; }
}