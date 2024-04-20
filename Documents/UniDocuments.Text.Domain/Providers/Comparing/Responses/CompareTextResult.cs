using Newtonsoft.Json;
using UniDocuments.Text.Domain.Services.Matching.Models;

namespace UniDocuments.Text.Domain.Providers.Comparing.Responses;

[Serializable]
public class CompareTextResult
{
    public string Text { get; }

    [JsonProperty("sv")]
    public double SimilarityValue { get; set; }
    
    [JsonProperty("me")]
    public List<MatchTextEntry>? MatchEntry { get; set; }

    public CompareTextResult(string text, double similarityValue, List<MatchTextEntry>? matchEntry)
    {
        Text = text;
        SimilarityValue = similarityValue;
        MatchEntry = matchEntry;
    }

    public static CompareTextResult NoSimilar(string text, double similarityValue)
    {
        return new CompareTextResult(text, similarityValue, null);
    }
}