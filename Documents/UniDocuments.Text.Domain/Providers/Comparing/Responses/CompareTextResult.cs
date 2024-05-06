using Newtonsoft.Json;

namespace UniDocuments.Text.Domain.Providers.Comparing.Responses;

[Serializable]
public class CompareTextResult
{
    public string Text { get; }

    [JsonProperty("sv")]
    public double SimilarityValue { get; set; }

    [JsonProperty("is")]
    public bool IsSuspicious { get; set; }

    public CompareTextResult(string text, double similarityValue, bool isSuspicious)
    {
        Text = text;
        SimilarityValue = similarityValue;
        IsSuspicious = isSuspicious;
    }
}