using System.Text.RegularExpressions;
using UniDocuments.Text.Algorithms.SequenceMatching;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Algorithms;
using UniDocuments.Text.Domain.Features;
using UniDocuments.Text.Domain.Services.Preprocessing;
using UniDocuments.Text.Features.Text;
using UniDocuments.Text.Plagiarism.Matching.Algorithm.Grams;
using UniDocuments.Text.Plagiarism.Matching.Algorithm.Services;
using UniDocuments.Text.Plagiarism.Matching.Data;
using UniDocuments.Text.Plagiarism.Matching.Data.Models;

namespace UniDocuments.Text.Plagiarism.Matching.Algorithm;

public partial class PlagiarismAlgorithmMatching : PlagiarismAlgorithm<PlagiarismResultMatching>
{
    private readonly IStopWordsService _stopWordsService;
    private readonly IMatchingOptionsProvider _optionsProvider;

    public PlagiarismAlgorithmMatching(IStopWordsService stopWordsService, IMatchingOptionsProvider optionsProvider)
    {
        _stopWordsService = stopWordsService;
        _optionsProvider = optionsProvider;
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

        var options = _optionsProvider.GetOptions();
        var originalGrams = CreateGrams(originalContent!.Content.ToRawText(), options);
        var comparingGrams = CreateGrams(comparingContent!.Content.ToRawText(), options);
        var matchingBlocks = originalGrams.GetMatchingBlocks(comparingGrams).ToList();
        var mergedBlocks = MapBlocksToTextPositions(matchingBlocks, originalGrams, comparingGrams, options);
        return PlagiarismResultMatching.FromBlocks(mergedBlocks);
    }

    private List<Gram> CreateGrams(string text, MatchingOptions options)
    {
        var n = options.NGram;
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
        List<SubSequence> matches, List<Gram> originalGrams, List<Gram> comparingGrams, MatchingOptions options)
    {
        MergeMatchingBlocks(matches, options);
        
        var result = new List<MatchEntry>();

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
            
            result.Add(new MatchEntry(
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