using UniDocuments.Text.Core;
using UniDocuments.Text.Core.Features;

namespace UniDocuments.Text.Plagiarism.Algorithms.Core;

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