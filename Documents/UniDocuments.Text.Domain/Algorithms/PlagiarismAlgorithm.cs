using UniDocuments.Text.Domain.Features;

namespace UniDocuments.Text.Domain.Algorithms;

public abstract class PlagiarismAlgorithm<T> : IPlagiarismAlgorithm<T> where T : IPlagiarismResult
{
    public abstract PlagiarismAlgorithmFeatureFlag FeatureFlag { get; }

    public IPlagiarismResult Perform(UniDocumentEntry entry)
    {
        return PerformExact(entry);
    }

    public abstract IEnumerable<UniDocumentFeatureFlag> GetRequiredFeatures();

    public abstract T PerformExact(UniDocumentEntry entry);
}