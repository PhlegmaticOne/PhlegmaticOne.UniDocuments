namespace UniDocuments.Text.Domain.Services.Fingerprinting.Options;

public class FingerprintOptions
{
    public double Baseline { get; set; }
    public FingerprintLevelOptions SentenceLevel { get; set; } = null!;
    public FingerprintLevelOptions ParagraphLevel { get; set; } = null!;
    public FingerprintLevelOptions DocumentLevel { get; set; } = null!;

    public FingerprintLevelOptions GetMatchingOptions(int wordsCount)
    {
        if (SentenceLevel.IsMatch(wordsCount))
        {
            return SentenceLevel;
        }

        return ParagraphLevel.IsMatch(wordsCount) ? ParagraphLevel : DocumentLevel;
    }
}