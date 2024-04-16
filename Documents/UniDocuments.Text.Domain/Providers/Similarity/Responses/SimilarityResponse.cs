using Newtonsoft.Json;
using UniDocuments.Text.Domain.Algorithms;

namespace UniDocuments.Text.Domain.Providers.Similarity.Responses;

[Serializable]
public class SimilarityResponse
{
    [JsonProperty("result")]
    private List<IPlagiarismResult> _plagiarismResults;
    
    [JsonConstructor]
    public SimilarityResponse(List<IPlagiarismResult> plagiarismResults)
    {
        _plagiarismResults = plagiarismResults;
    }
    
    public SimilarityResponse()
    {
        _plagiarismResults = new List<IPlagiarismResult>();
    }

    public void AddResult(IPlagiarismResult result)
    {
        _plagiarismResults.Add(result);
    }

    public IReadOnlyList<IPlagiarismResult> GetResults()
    {
        return _plagiarismResults;
    }
}