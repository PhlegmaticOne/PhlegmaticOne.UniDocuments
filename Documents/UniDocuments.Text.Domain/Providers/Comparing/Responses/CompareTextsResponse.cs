using Newtonsoft.Json;

namespace UniDocuments.Text.Domain.Providers.Comparing.Responses;

[Serializable]
public class CompareTextsResponse
{
    [JsonProperty]
    public string SourceText { get; set; }
    [JsonProperty]
    public List<CompareTextResult> SimilarityResults { get; }

    public CompareTextsResponse(string sourceText) : this(sourceText, new List<CompareTextResult>())
    {
    }

    [JsonConstructor]
    public CompareTextsResponse(string sourceText, List<CompareTextResult> similarityResults)
    {
        SourceText = sourceText;
        SimilarityResults = similarityResults;
    }

    public void AddResult(CompareTextResult compareResult)
    {
        SimilarityResults.Add(compareResult);
    }
}