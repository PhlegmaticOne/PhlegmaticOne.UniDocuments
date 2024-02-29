using PhlegmaticOne.UniDocuments.Documents.Core.Algorithms;

namespace PhlegmaticOne.UniDocuments.Documents.Algorithms.Cosine;

public class PlagiarismResultCosine : IPlagiarismResult
{
    public PlagiarismResultCosine(double cosineSimilarity)
    {
        CosineSimilarity = cosineSimilarity;
    }

    public double CosineSimilarity { get; }

    public object GetRawValue()
    {
        return CosineSimilarity;
    }
}
