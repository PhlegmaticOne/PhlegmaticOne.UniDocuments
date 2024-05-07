namespace UniDocuments.Text.Domain.Services.Fingerprinting.Options;

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