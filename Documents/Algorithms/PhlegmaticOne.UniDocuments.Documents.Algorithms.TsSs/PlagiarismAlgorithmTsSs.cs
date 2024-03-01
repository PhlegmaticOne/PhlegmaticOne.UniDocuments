using PhlegmaticOne.UniDocuments.Documents.Core;
using PhlegmaticOne.UniDocuments.Documents.Core.Algorithms;
using PhlegmaticOne.UniDocuments.Documents.Core.Features.Content;
using PhlegmaticOne.UniDocuments.Math;
using PhlegmaticOne.UniDocuments.TextProcessing.Base;
using PhlegmaticOne.UniDocuments.TextProcessing.Helpers;
using PhlegmaticOne.UniDocuments.TextProcessing.Models;

namespace PhlegmaticOne.UniDocuments.Documents.Algorithms.TsSs;

public class PlagiarismAlgorithmTsSs : IPlagiarismAlgorithm
{
    private readonly ITextPreprocessor _textPreprocessor;

    public PlagiarismAlgorithmTsSs(ITextPreprocessor textPreprocessor)
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
        var originalVector = UniVector<int>.FromEnumerating(allKeys, w => originalWords.GetValueOrDefault(w, 0));
        var comparingVector = UniVector<int>.FromEnumerating(allKeys, w => comparingWords.GetValueOrDefault(w, 0));
        var metric = CalculateTsSs(originalVector, comparingVector);
        
        return new PlagiarismResultTsSs(metric);
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