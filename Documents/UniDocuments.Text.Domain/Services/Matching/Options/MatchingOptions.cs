namespace UniDocuments.Text.Domain.Services.Matching.Options;

public class MatchingOptions
{
    public int NGram { get; set; }
    public int Threshold { get; set; }
    public int MinDistance { get; set; }
}