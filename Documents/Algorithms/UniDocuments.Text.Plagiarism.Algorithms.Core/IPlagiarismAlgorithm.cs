using UniDocuments.Text.Core;
using UniDocuments.Text.Core.Features;

namespace UniDocuments.Text.Plagiarism.Algorithms.Core;

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