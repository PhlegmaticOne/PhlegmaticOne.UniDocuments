using UniDocuments.Text.Domain.Services.BaseMetrics;
using UniDocuments.Text.Domain.Services.Preprocessing;
using UniDocuments.Text.Services.BaseMetrics.Infrastructure;
using UniDocuments.Text.Services.BaseMetrics.Models;

namespace UniDocuments.Text.Services.BaseMetrics;

[BaseMetricDefault]
public class TextSimilarityBaseMetricCosine : ITextSimilarityBaseMetric
{
    private readonly ITextPreprocessor _textPreprocessor;

    public TextSimilarityBaseMetricCosine(ITextPreprocessor textPreprocessor)
    {
        _textPreprocessor = textPreprocessor;
    }

    public string Name => "cosine";

    public double Calculate(string sourceText, string suspiciousText)
    {
        var sourceDictionary = CreateWordsDictionary(sourceText);
        var suspiciousDictionary = CreateWordsDictionary(suspiciousText);
        var union = sourceDictionary.Merge(suspiciousDictionary);
        
        var sourceVector = UniVector<int>.FromEnumerating(union, w => sourceDictionary.GetWordEntriesCount(w));
        var suspiciousVector = UniVector<int>.FromEnumerating(union, w => suspiciousDictionary.GetWordEntriesCount(w));

        return sourceVector.Cosine(suspiciousVector);
    }

    public bool IsSuspicious(double metricValue)
    {
        return metricValue > 0.5;
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