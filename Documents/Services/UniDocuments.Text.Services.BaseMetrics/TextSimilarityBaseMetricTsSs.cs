using UniDocuments.Text.Domain.Services.BaseMetrics;
using UniDocuments.Text.Domain.Services.BaseMetrics.Options;
using UniDocuments.Text.Domain.Services.Preprocessing;
using UniDocuments.Text.Services.BaseMetrics.Infrastructure;
using UniDocuments.Text.Services.BaseMetrics.Models;

namespace UniDocuments.Text.Services.BaseMetrics;

public class TextSimilarityBaseMetricTsSs : ITextSimilarityBaseMetric
{
    private readonly ITextPreprocessor _textPreprocessor;
    private readonly double _baseLine;

    public TextSimilarityBaseMetricTsSs(ITextPreprocessor textPreprocessor, IMetricBaselinesOptionsProvider optionsProvider)
    {
        _textPreprocessor = textPreprocessor;
        _baseLine = optionsProvider.GetOptions()[Name];
    }

    public string Name => "ts-ss";

    public double Calculate(string sourceText, string suspiciousText)
    {
        var sourceDictionary = CreateWordsDictionary(sourceText);
        var suspiciousDictionary = CreateWordsDictionary(suspiciousText);
        var union = sourceDictionary.Merge(suspiciousDictionary);
        
        var sourceVector = UniVector<int>.FromEnumerating(union, w => sourceDictionary.GetWordEntriesCount(w));
        var suspiciousVector = UniVector<int>.FromEnumerating(union, w => suspiciousDictionary.GetWordEntriesCount(w));

        return CalculateTsSs(sourceVector, suspiciousVector);
    }

    public bool IsSuspicious(double metricValue)
    {
        return metricValue < _baseLine;
    }
    
    private static double CalculateTsSs(UniVector<int> source, UniVector<int> suspicious)
    {
        var theta = ThetaAngle(source, suspicious);
        var normOriginal = source.Norm();
        var normComparing = suspicious.Norm();
        
        var distance = source.EuclideanDistance(suspicious);
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

    private DocumentWordsDictionary CreateWordsDictionary(string text)
    {
        var words = _textPreprocessor.Preprocess(new PreprocessorTextInput
        {
            Text = text
        });

        return new DocumentWordsDictionary(words.Words);
    }
}