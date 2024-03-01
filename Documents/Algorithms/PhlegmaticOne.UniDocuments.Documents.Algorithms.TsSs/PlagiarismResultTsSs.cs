using PhlegmaticOne.UniDocuments.Documents.Core.Algorithms;

namespace PhlegmaticOne.UniDocuments.Documents.Algorithms.TsSs;

public class PlagiarismResultTsSs : IPlagiarismResult
{
    public double TsSsValue { get; }

    public PlagiarismResultTsSs(double tsSsValue)
    {
        TsSsValue = tsSsValue;
    }
    
    public object GetRawValue()
    {
        return TsSsValue;
    }
}