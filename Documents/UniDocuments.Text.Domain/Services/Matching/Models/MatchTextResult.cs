namespace UniDocuments.Text.Domain.Services.Matching.Models;

[Serializable]
public class MatchTextResult
{
    public string SourceText { get; }
    public List<MatchTextEntry> MatchEntries { get; }

    public static MatchTextResult Empty => new(string.Empty, new List<MatchTextEntry>());

    public MatchTextResult(string sourceText, List<MatchTextEntry> matchEntries)
    {
        SourceText = sourceText;
        MatchEntries = matchEntries;
    }
}