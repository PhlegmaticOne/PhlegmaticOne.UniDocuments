using UniDocuments.Text.Core;
using UniDocuments.Text.Core.Algorithms;
using UniDocuments.Text.Core.Features.Content;
using UniDocuments.Text.Math;
using UniDocuments.Text.Plagiarism.Cosine.Data;
using UniDocuments.Text.Processing.Preprocessing.Base;
using UniDocuments.Text.Processing.Preprocessing.Models;
using UniDocuments.Text.Processing.WordsDictionary;

namespace UniDocuments.Text.Plagiarism.Cosine.Algorithm;

public class PlagiarismAlgorithmCosineSimilarity : PlagiarismAlgorithm<PlagiarismResultCosine>
{
    private readonly ITextPreprocessor _textPreprocessor;

    public PlagiarismAlgorithmCosineSimilarity(ITextPreprocessor textPreprocessor)
    {
        _textPreprocessor = textPreprocessor;
    }

    public override PlagiarismResultCosine PerformExact(UniDocument original, UniDocument comparing)
    {
        if (!comparing.TryGetFeature<IUniDocumentFeatureText>(out var comparingContent) ||
            !original.TryGetFeature<IUniDocumentFeatureText>(out var originalContent))
        {
            return PlagiarismResultCosine.Error;
        }

        var proceedOriginalText = _textPreprocessor.Preprocess(new PreprocessorTextInput
        {
            Text = originalContent!.GetText()
        });

        var proceedComparingText = _textPreprocessor.Preprocess(new PreprocessorTextInput
        {
            Text = comparingContent!.GetText()
        });

        var originalWords = new DocumentWordsDictionary(proceedOriginalText.Words);
        var comparingWords = new DocumentWordsDictionary(proceedComparingText.Words);
        var allKeys = originalWords.Merge(comparingWords);
        var originalVector = UniVector<int>.FromEnumerating(allKeys, w => originalWords.GetWordEntriesCount(w));
        var comparingVector = UniVector<int>.FromEnumerating(allKeys, w => comparingWords.GetWordEntriesCount(w));
        var metric = originalVector.Cosine(comparingVector);
        return PlagiarismResultCosine.FromCosine(metric);
    }
}
