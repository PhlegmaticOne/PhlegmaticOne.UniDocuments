﻿using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Services.BaseMetrics;
using UniDocuments.Text.Domain.Services.Fingerprinting.Options;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;

namespace UniDocuments.Text.Services.BaseMetrics;

public class TextSimilarityBaseMetricFingerprint : ITextSimilarityBaseMetric
{
    private readonly IFingerprintAlgorithm _fingerprintAlgorithm;
    private readonly IFingerprintOptionsProvider _optionsProvider;

    public TextSimilarityBaseMetricFingerprint(
        IFingerprintAlgorithm fingerprintAlgorithm, 
        IFingerprintOptionsProvider optionsProvider)
    {
        _fingerprintAlgorithm = fingerprintAlgorithm;
        _optionsProvider = optionsProvider;
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
        return metricValue > 0.5;
    }
}