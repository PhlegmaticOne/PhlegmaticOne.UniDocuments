using System.Text.RegularExpressions;
using UniDocuments.Text.Algorithms.SequenceMatching;
using UniDocuments.Text.Domain.Services.Matching;
using UniDocuments.Text.Domain.Services.Matching.Models;
using UniDocuments.Text.Domain.Services.Matching.Options;
using UniDocuments.Text.Domain.Services.Preprocessing;
using UniDocuments.Text.Services.Matching.Grams;

namespace UniDocuments.Text.Services.Matching;

public partial class TextMatchingAlgorithm : ITextMatchingAlgorithm
{
    private readonly IStopWordsService _stopWordsService;

    public TextMatchingAlgorithm(IStopWordsService stopWordsService)
    {
        _stopWordsService = stopWordsService;
    }
    
    public MatchTextResult Match(string source, string suspicious, MatchingOptions options)
    {
        if (string.IsNullOrWhiteSpace(suspicious))
        {
            return MatchTextResult.Empty;
        }
        
        var originalGrams = CreateGrams(source, options);
        var comparingGrams = CreateGrams(suspicious, options);
        var matchingBlocks = originalGrams.GetMatchingBlocks(comparingGrams).ToList();
        var mergedBlocks = MapBlocksToTextPositions(matchingBlocks, originalGrams, comparingGrams, options);
        return new MatchTextResult(suspicious, mergedBlocks);
    }

    private List<Gram> CreateGrams(string text, MatchingOptions options)
    {
        var n = options.NGram;
        var grams = new List<Gram>();
        
        var words = WordsRegex().Matches(text)
            .Where(x => _stopWordsService.IsStopWord(x.Value) == false)
            .Select(x => new GramEntry(x.Value, x.Index, x.Length))
            .ToArray();

        if (words.Length < n)
        {
            var gram = new Gram(words, 0, words.Length);
            return new List<Gram> { gram };
        }
        
        for (var i = 0; i < words.Length - n + 1; i++)
        {
            var gram = new Gram(words, i, i + n);
            grams.Add(gram);
        }

        return grams;
    }

    private static List<MatchTextEntry> MapBlocksToTextPositions(
        List<SubSequence> matches, List<Gram> originalGrams, List<Gram> comparingGrams, MatchingOptions options)
    {
        MergeMatchingBlocks(matches, options);
        
        var result = new List<MatchTextEntry>();

        foreach (var match in matches)
        {
            if (IsMatchValuable(match, options) == false)
            {
                continue;
            }
            
            var originalTextFragmentStartIndex = originalGrams[match.LeftIndex].GetMinPosition();
            var originalTextFragmentEndIndex = originalGrams[match.LeftEndIndex].GetMaxPosition();
            var comparingTextFragmentStartIndex = comparingGrams[match.RightIndex].GetMinPosition();
            var comparingTextFragmentEndIndex = comparingGrams[match.RightEndIndex].GetMaxPosition();
            
            result.Add(new MatchTextEntry(
                originalTextFragmentStartIndex, originalTextFragmentEndIndex,
                comparingTextFragmentStartIndex, comparingTextFragmentEndIndex));
        }

        return result;
    }

    private static void MergeMatchingBlocks(IList<SubSequence> matches, MatchingOptions options)
    {
        if (matches.Count == 1)
        {
            return;
        }

        var minDistance = options.MinDistance;
        
        for (var i = matches.Count - 1; i > 0; i--)
        {
            var previous = matches[i - 1];
            var current = matches[i];

            if (previous.RightIndex + previous.RightLength + minDistance >= current.RightIndex &&
                previous.LeftIndex + previous.LeftLength + minDistance >= current.LeftIndex)
            {
                previous.RightLength = current.RightIndex - previous.RightIndex + current.RightLength;
                previous.LeftLength = current.LeftIndex - previous.LeftIndex + current.LeftLength;
                matches[i - 1] = previous;
                matches.Remove(current);
            }
        }
    }

    private static bool IsMatchValuable(in SubSequence match, MatchingOptions options)
    {
        return match.Size > options.Threshold;
    }

    [GeneratedRegex("[^\\s\\.,\\?\\/\\\\#!$%\\^&\\*;:{}=\\-_`~()\\\"]{2,}")]
    private static partial Regex WordsRegex();


}