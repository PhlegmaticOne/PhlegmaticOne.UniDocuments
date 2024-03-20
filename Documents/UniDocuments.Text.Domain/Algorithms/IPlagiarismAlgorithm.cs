using UniDocuments.Text.Domain.Features;

namespace UniDocuments.Text.Domain.Algorithms;

public interface IPlagiarismAlgorithm
{
    PlagiarismAlgorithmFeatureFlag FeatureFlag { get; }
    IPlagiarismResult Perform(UniDocumentEntry entry);
    IEnumerable<UniDocumentFeatureFlag> GetRequiredFeatures();
}

internal interface IPlagiarismAlgorithm<out T> : IPlagiarismAlgorithm where T : IPlagiarismResult
{
    T PerformExact(UniDocumentEntry entry);
}