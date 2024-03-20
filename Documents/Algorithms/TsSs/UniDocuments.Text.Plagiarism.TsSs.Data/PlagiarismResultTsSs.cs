using UniDocuments.Text.Domain.Algorithms;

namespace UniDocuments.Text.Plagiarism.TsSs.Data;

[Serializable]
public class PlagiarismResultTsSs : IPlagiarismResult
{
    public PlagiarismResultTsSs(double tsSsValue, bool isSucceed)
    {
        TsSsValue = tsSsValue;
        IsSucceed = isSucceed;
    }

    public static PlagiarismResultTsSs Error => new(double.MinValue, false);
    public static PlagiarismResultTsSs FromTsSs(double tsSsValue) => new(tsSsValue, true);
    public double TsSsValue { get; }
    public bool IsSucceed { get; }
    
    public override string ToString()
    {
        return $"TS-SS: {TsSsValue:F}";
    }
}