using UniDocuments.Text.Algorithms;
using UniDocuments.Text.Domain.Services.Preprocessing;
using UniDocuments.Text.Domain.Services.StreamReading;
using UniDocuments.Text.Features.Fingerprint.Models;

namespace UniDocuments.Text.Features.Fingerprint.Services;

public class FingerprintWinnowingAlgorithm : IFingerprintAlgorithm
{
    private const int Gram = 30;
    private const int Window = 64;
    
    private readonly ITextPreprocessor _textPreprocessor;
    private readonly IFingerprintHash _fingerprintHash;

    public FingerprintWinnowingAlgorithm(ITextPreprocessor textPreprocessor, IFingerprintHash fingerprintHash)
    {
        _textPreprocessor = textPreprocessor;
        _fingerprintHash = fingerprintHash;
    }
    
    public DocumentFingerprint Fingerprint(StreamContentReadResult text)
    {
        var processed = _textPreprocessor.Preprocess(new PreprocessorTextInput
        {
            Text = text.ToRawText()
        });

        var concat = string.Concat(processed.Words);
        var fingerprints = FingerprintText(concat);
        var winnowedFingerprints = WinnowFingerprints(fingerprints);
        return new DocumentFingerprint(winnowedFingerprints);
    }

    private List<uint> FingerprintText(string text)
    {
        var fingerprints = new List<uint>();

        for (var i = 0; i < text.Length - Gram + 1; i++)
        {
            var hash = _fingerprintHash.GetHash(text, i, Gram);
            fingerprints.Add(hash);
        }

        return fingerprints;
    }

    private static HashSet<uint> WinnowFingerprints(List<uint> fingerprints)
    {
        var window = new HashSet<uint>();
        var previousMin = 0;
            
        for (var i = 0; i < fingerprints.Count - Window; i++)
        {
            var minIndexInRange = fingerprints.GetMinIndexInRange(i, i + Window);
            var currentMin = i + minIndexInRange;

            if (currentMin != previousMin)
            {
                window.Add(fingerprints[currentMin]);
                previousMin = currentMin;
            }
        }

        return window;
    }
}