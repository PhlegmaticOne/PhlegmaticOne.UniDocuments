using UniDocuments.Text.Algorithms;
using UniDocuments.Text.Domain.Services.Preprocessing;
using UniDocuments.Text.Domain.Services.StreamReading;
using UniDocuments.Text.Features.Fingerprint.Models;

namespace UniDocuments.Text.Features.Fingerprint.Services;

public class FingerprintWinnowingAlgorithm : IFingerprintAlgorithm
{
    private readonly ITextPreprocessor _textPreprocessor;
    private readonly IFingerprintHash _fingerprintHash;
    private readonly IFingerprintOptionsProvider _optionsProvider;

    public FingerprintWinnowingAlgorithm(
        ITextPreprocessor textPreprocessor, 
        IFingerprintHash fingerprintHash,
        IFingerprintOptionsProvider optionsProvider)
    {
        _textPreprocessor = textPreprocessor;
        _fingerprintHash = fingerprintHash;
        _optionsProvider = optionsProvider;
    }
    
    public DocumentFingerprint Fingerprint(StreamContentReadResult text)
    {
        var options = _optionsProvider.GetOptions();

        var processed = _textPreprocessor.Preprocess(new PreprocessorTextInput
        {
            Text = text.ToRawTextWithMinLength(options.MinWords)
        });

        var concat = string.Concat(processed.Words);
        var levelOptions = options.GetMatchingOptions(processed.Words.Length);
        var fingerprints = FingerprintText(concat, levelOptions.GramSize);
        var winnowedFingerprints = WinnowFingerprints(fingerprints, levelOptions.WindowSize);
        return new DocumentFingerprint(winnowedFingerprints);
    }

    private List<uint> FingerprintText(string text, int gramSize)
    {
        var fingerprints = new List<uint>();

        for (var i = 0; i < text.Length - gramSize + 1; i++)
        {
            var hash = _fingerprintHash.GetHash(text, i, gramSize);
            fingerprints.Add(hash);
        }

        return fingerprints;
    }

    private static HashSet<uint> WinnowFingerprints(List<uint> fingerprints, int windowSize)
    {
        var window = new HashSet<uint>();
        var previousMin = 0;
            
        for (var i = 0; i < fingerprints.Count - windowSize; i++)
        {
            var minIndexInRange = fingerprints.GetMinIndexInRange(i, i + windowSize);
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