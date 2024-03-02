namespace UniDocuments.Text.Algorithms.SequenceMatching;

public static class SequenceMatcherExtensions
{
    public static IEnumerable<SubSequence> GetMatchingBlocks<T>(this IList<T> left, IList<T> right) where T : IEquatable<T>
    {
        return SequenceMatcher.GetMatchingBlocks(left, right);
    }
}