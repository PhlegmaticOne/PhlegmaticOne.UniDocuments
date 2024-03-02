namespace UniDocuments.Text.Core.Algorithms;

public interface IPlagiarismAlgorithm
{
    IPlagiarismResult Perform(UniDocument original, UniDocument comparing);
}

internal interface IPlagiarismAlgorithm<out T> : IPlagiarismAlgorithm where T : IPlagiarismResult
{
    T PerformExact(UniDocument original, UniDocument comparing);
}