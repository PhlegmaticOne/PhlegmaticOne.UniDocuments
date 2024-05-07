using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Services.BaseMetrics;
using UniDocuments.Text.Domain.Services.BaseMetrics.Options;
using UniDocuments.Text.Domain.Services.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Options;

namespace UniDocuments.Text.Services.BaseMetrics;

public class TextSimilarityBaseMetricFingerprint : ITextSimilarityBaseMetric
{
    private readonly double _baseLine;
    
    private readonly IFingerprintAlgorithm _fingerprintAlgorithm;
    private readonly IFingerprintOptionsProvider _optionsProvider;

    public TextSimilarityBaseMetricFingerprint(
        IFingerprintAlgorithm fingerprintAlgorithm, 
        IFingerprintOptionsProvider optionsProvider,
        IMetricBaselinesOptionsProvider metricBaselinesOptionsProvider)
    {
        _fingerprintAlgorithm = fingerprintAlgorithm;
        _optionsProvider = optionsProvider;
        _baseLine = metricBaselinesOptionsProvider.GetOptions()[Name];
    }

    public string Name => "fingerprint";

    public double Calculate(string sourceText, string suspiciousText)
    {
        var options = _optionsProvider.GetOptions();
        
        var sourceFingerprint = _fingerprintAlgorithm.Fingerprint(UniDocument.FromString(sourceText), options);
        
        var suspiciousFingerprint = _fingerprintAlgorithm.Fingerprint(UniDocument.FromString(suspiciousText), options);

        return sourceFingerprint.CalculateJaccard(suspiciousFingerprint);
    }

    public bool IsSuspicious(double metricValue)
    {
        return metricValue > _baseLine;
    }
}