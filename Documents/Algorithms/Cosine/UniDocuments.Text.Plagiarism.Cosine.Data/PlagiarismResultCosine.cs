using UniDocuments.Text.Plagiarism.Algorithms.Core;

namespace UniDocuments.Text.Plagiarism.Cosine.Data;

[Serializable]
public class PlagiarismResultCosine : IPlagiarismResult
{
    public PlagiarismResultCosine(double cosineSimilarity, bool isSucceed)
    {
        CosineSimilarity = cosineSimilarity;
        IsSucceed = isSucceed;
    }

    public static PlagiarismResultCosine Error => new(double.MinValue, false);
    public static PlagiarismResultCosine FromCosine(double cosineSimilarity) => new(cosineSimilarity, true);
    
    public bool IsSucceed { get; }
    public double CosineSimilarity { get; }

    public override string ToString()
    {
        return $"Cosine: {CosineSimilarity:F}";
    }
}
