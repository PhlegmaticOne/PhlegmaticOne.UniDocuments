using UniDocuments.Text.Core;
using UniDocuments.Text.Core.Algorithms;
using UniDocuments.Text.Core.Features.Content;
using UniDocuments.Text.Math;
using UniDocuments.Text.Plagiarism.TsSs.Data;
using UniDocuments.Text.Processing.Preprocessing.Base;
using UniDocuments.Text.Processing.Preprocessing.Models;
using UniDocuments.Text.Processing.WordsDictionary;

namespace UniDocuments.Text.Plagiarism.TsSs.Algorithm;

public class PlagiarismAlgorithmTsSs : IPlagiarismAlgorithm
{
    private readonly ITextPreprocessor _textPreprocessor;

    public PlagiarismAlgorithmTsSs(ITextPreprocessor textPreprocessor)
    {
        _textPreprocessor = textPreprocessor;
    }
    
    public IPlagiarismResult Perform(UniDocument comparing, UniDocument original)
    {
        if (!comparing.TryGetFeature<IUniDocumentFeatureText>(out var comparingContent) ||
            !original.TryGetFeature<IUniDocumentFeatureText>(out var originalContent))
        {
            return PlagiarismResultTsSs.Error;
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
        var metric = CalculateTsSs(originalVector, comparingVector);
        return PlagiarismResultTsSs.FromTsSs(metric);
    }

    private static double CalculateTsSs(UniVector<int> original, UniVector<int> comparing)
    {
        var theta = ThetaAngle(original, comparing);
        var normOriginal = original.Norm();
        var normComparing = comparing.Norm();
        
        var distance = original.EuclideanDistance(comparing);
        var magnitudeDifference = (normComparing - normOriginal).Abs();
        var ss = theta.SegmentSquare(distance + magnitudeDifference);
        var ts = theta.TriangleSquare(normOriginal, normComparing);

        return ts * ss;
    }

    private static UniAngle ThetaAngle(UniVector<int> original, UniVector<int> comparing)
    {
        var cosine = original.Cosine(comparing);
        return UniAngle.ArcCos(cosine) + UniAngle.FromDegrees(10);
    }
}