using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Services.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Models;
using UniDocuments.Text.Domain.Services.Fingerprinting.Options;
using UniDocuments.Text.Domain.Services.Preprocessing.Preprocessor;
using UniDocuments.Text.Services.Fingerprinting.Extensions;

namespace UniDocuments.Text.Services.Fingerprinting;

public class FingerprintWinnowingAlgorithm : IFingerprintAlgorithm
{
    private readonly ITextPreprocessor _textPreprocessor;
    private readonly IFingerprintHash _fingerprintHash;

    public FingerprintWinnowingAlgorithm(
        ITextPreprocessor textPreprocessor, 
        IFingerprintHash fingerprintHash)
    {
        _textPreprocessor = textPreprocessor;
        _fingerprintHash = fingerprintHash;
    }
    
    public TextFingerprint Fingerprint(UniDocument document, FingerprintOptions options)
    {
        var processed = _textPreprocessor.Preprocess(new PreprocessorTextInput
        {
            Text = document.Content.ToRawText()
        });

        var concat = string.Concat(processed.Words);
        var levelOptions = options.Options;
        var fingerprints = FingerprintText(concat, levelOptions.GramSize);
        var winnowedFingerprints = WinnowFingerprints(fingerprints, levelOptions.WindowSize);
        return new TextFingerprint(document.Id, winnowedFingerprints);
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