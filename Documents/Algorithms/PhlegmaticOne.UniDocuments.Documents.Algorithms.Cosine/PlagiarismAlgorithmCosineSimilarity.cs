using PhlegmaticOne.UniDocuments.Documents.Algorithms.Text.Helpers;
using PhlegmaticOne.UniDocuments.Documents.Algorithms.TextPreprocessing.Base;
using PhlegmaticOne.UniDocuments.Documents.Algorithms.TextPreprocessing.Models;
using PhlegmaticOne.UniDocuments.Documents.Core;
using PhlegmaticOne.UniDocuments.Documents.Core.Algorithms;
using PhlegmaticOne.UniDocuments.Documents.Core.Features.Content;
using PhlegmaticOne.UniDocuments.Math;

namespace PhlegmaticOne.UniDocuments.Documents.Algorithms.Cosine;

public class PlagiarismAlgorithmCosineSimilarity : IPlagiarismAlgorithm
{
    private readonly ITextPreprocessor _textPreprocessor;

    public PlagiarismAlgorithmCosineSimilarity(ITextPreprocessor textPreprocessor)
    {
        _textPreprocessor = textPreprocessor;
    }

    public IPlagiarismResult Perform(UniDocument comparing, UniDocument original)
    {
        if (!comparing.TryGetFeature<IUniDocumentTextFeature>(out var comparingContent) ||
            !original.TryGetFeature<IUniDocumentTextFeature>(out var originalContent))
        {
            return PlagiarismResultError.FromMessage("No content provided");
        }

        var proceedOriginalText = _textPreprocessor.ProcessText(new TextInput
        {
            Text = originalContent!.GetText()
        });

        var proceedComparingText = _textPreprocessor.ProcessText(new TextInput
        {
            Text = comparingContent!.GetText()
        });

        var originalWords = WordsDictionary.CreateWordsDictionary(proceedOriginalText.Words);
        var comparingWords = WordsDictionary.CreateWordsDictionary(proceedComparingText.Words);
        var allKeys = WordsDictionary.GetAllKeys(originalWords, comparingWords);

        var originalVector = UniVector<int>.FromEnumerating(allKeys, word =>
        {
            return SelectWordEntriesCount(word, originalWords);
        });

        var comparingVector = UniVector<int>.FromEnumerating(allKeys, word =>
        {
            return SelectWordEntriesCount(word, comparingWords);
        });

        var normOriginal = originalVector.Norm();
        var normComparing = comparingVector.Norm();
        var dot = originalVector.Dot(comparingVector);
        var metric = dot / (normOriginal * normComparing);

        return new PlagiarismResultCosine(metric);
    }

    private static int SelectWordEntriesCount(string word, Dictionary<string, int> words)
    {
        if (words.TryGetValue(word, out var result))
        {
            return result;
        }

        return 0;
    }
}
