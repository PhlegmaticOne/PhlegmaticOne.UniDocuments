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

public class FingerprintLevelOptions
{
    public int[] WordsCount { get; set; } = null!;
    public int WindowSize { get; set; }
    public int GramSize { get; set; }

    public bool IsMatch(int wordsCount)
    {
        if (wordsCount < WordsCount[0])
        {
            return false;
        }

        return WordsCount[1] == -1 || wordsCount <= WordsCount[1];
    }
}