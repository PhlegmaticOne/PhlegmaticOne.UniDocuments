using Newtonsoft.Json;
using UniDocuments.Text.Domain.Services.Matching.Models;

namespace UniDocuments.Text.Domain.Providers.Matching.Responses;

[Serializable]
public class MatchTextsResponse
{
    public string Text { get; }
    public List<MatchTextResult> MatchResults { get; }

    public MatchTextsResponse(string text) : this(text, new List<MatchTextResult>()) { }

    [JsonConstructor]
    public MatchTextsResponse(string text, List<MatchTextResult> matchResults)
    {
        Text = text;
        MatchResults = matchResults;
    }

    public void AddResult(MatchTextResult result)
    {
        MatchResults.Add(result);
    }
}