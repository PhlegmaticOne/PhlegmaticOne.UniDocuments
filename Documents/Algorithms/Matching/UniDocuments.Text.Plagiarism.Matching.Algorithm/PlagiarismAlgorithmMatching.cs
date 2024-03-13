using System.Text.RegularExpressions;
using UniDocuments.Text.Algorithms.SequenceMatching;
using UniDocuments.Text.Core;
using UniDocuments.Text.Core.Features;
using UniDocuments.Text.Features.Text;
using UniDocuments.Text.Plagiarism.Algorithms.Core;
using UniDocuments.Text.Plagiarism.Matching.Algorithm.Grams;
using UniDocuments.Text.Plagiarism.Matching.Data;
using UniDocuments.Text.Plagiarism.Matching.Data.Models;
using UniDocuments.Text.Processing.StopWords;

namespace UniDocuments.Text.Plagiarism.Matching.Algorithm;

public partial class PlagiarismAlgorithmMatching : PlagiarismAlgorithm<PlagiarismResultMatching>
{
    private const int N = 3;
    private const int Threshold = 8;
    private const int MinDistance = 8;
    
    private readonly IStopWordsService _stopWordsService;

    public PlagiarismAlgorithmMatching(IStopWordsService stopWordsService)
    {
        _stopWordsService = stopWordsService;
    }

    public override PlagiarismAlgorithmFeatureFlag FeatureFlag => PlagiarismAlgorithmMatchingFeatureFlag.Instance;

    public override IEnumerable<UniDocumentFeatureFlag> GetRequiredFeatures()
    {
        yield return UniDocumentFeatureTextFlag.Instance;
    }

    public override PlagiarismResultMatching PerformExact(UniDocumentEntry entry)
    {
        if (!entry.Comparing.TryGetFeature<UniDocumentFeatureText>(out var comparingContent) ||
            !entry.Original.TryGetFeature<UniDocumentFeatureText>(out var originalContent))
        {
            return PlagiarismResultMatching.Error;
        }

        var originalGrams = CreateGrams(originalContent!.GetText(), N);
        var comparingGrams = CreateGrams(comparingContent!.GetText(), N);
        var matchingBlocks = originalGrams.GetMatchingBlocks(comparingGrams).ToList();
        var mergedBlocks = MapBlocksToTextPositions(matchingBlocks, originalGrams, comparingGrams);
        return PlagiarismResultMatching.FromBlocks(mergedBlocks);
    }

    private List<Gram> CreateGrams(string text, int n)
    {
        var grams = new List<Gram>();
        
        var words = WordsRegex().Matches(text)
            .Where(x => _stopWordsService.IsStopWord(x.Value) == false)
            .Select(x => new GramEntry(x.Value, x.Index, x.Length))
            .ToArray();
        
        for (var i = 0; i < words.Length - n + 1; i++)
        {
            var gram = new Gram(words, i, i + n);
            grams.Add(gram);
        }

        return grams;
    }

    private static List<MatchEntry> MapBlocksToTextPositions(
        List<SubSequence> matches, List<Gram> originalGrams, List<Gram> comparingGrams)
    {
        MergeMatchingBlocks(matches);
        
        var result = new List<MatchEntry>();

        foreach (var match in matches)
        {
            if (IsMatchValuable(match) == false)
            {
                continue;
            }
            
            var originalTextFragmentStartIndex = originalGrams[match.LeftIndex].GetMinPosition();
            var originalTextFragmentEndIndex = originalGrams[match.LeftEndIndex].GetMaxPosition();
            var comparingTextFragmentStartIndex = comparingGrams[match.RightIndex].GetMinPosition();
            var comparingTextFragmentEndIndex = comparingGrams[match.RightEndIndex].GetMaxPosition();
            
            result.Add(new MatchEntry(
                originalTextFragmentStartIndex, originalTextFragmentEndIndex,
                comparingTextFragmentStartIndex, comparingTextFragmentEndIndex));
        }

        return result;
    }

    private static void MergeMatchingBlocks(IList<SubSequence> matches)
    {
        if (matches.Count == 1)
        {
            return;
        }

        for (var i = matches.Count - 1; i > 0; i--)
        {
            var previous = matches[i - 1];
            var current = matches[i];

            if (previous.RightIndex + previous.RightLength + MinDistance >= current.RightIndex &&
                previous.LeftIndex + previous.LeftLength + MinDistance >= current.LeftIndex)
            {
                previous.RightLength = current.RightIndex - previous.RightIndex + current.RightLength;
                previous.LeftLength = current.LeftIndex - previous.LeftIndex + current.LeftLength;
                matches[i - 1] = previous;
                matches.Remove(current);
            }
        }
    }

    private static bool IsMatchValuable(in SubSequence match)
    {
        return match.Size > Threshold;
    }

    [GeneratedRegex("[^\\s\\.,\\?\\/\\\\#!$%\\^&\\*;:{}=\\-_`~()\\\"]{2,}")]
    private static partial Regex WordsRegex();
}