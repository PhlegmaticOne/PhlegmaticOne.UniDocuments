namespace UniDocuments.Text.Core.Algorithms;

public abstract class PlagiarismAlgorithm<T> : IPlagiarismAlgorithm<T> where T : IPlagiarismResult
{
    public IPlagiarismResult Perform(UniDocument original, UniDocument comparing)
    {
        return PerformExact(original, comparing);
    }
    
    public abstract T PerformExact(UniDocument original, UniDocument comparing);
}