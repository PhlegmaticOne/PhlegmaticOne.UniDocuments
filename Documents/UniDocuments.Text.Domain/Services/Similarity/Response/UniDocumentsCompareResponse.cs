using Newtonsoft.Json;
using UniDocuments.Text.Domain.Algorithms;

namespace UniDocuments.Text.Domain.Services.Similarity.Response;

[Serializable]
public class UniDocumentsCompareResponse
{
    [JsonProperty("result")]
    private List<IPlagiarismResult> _plagiarismResults;
    
    [JsonConstructor]
    public UniDocumentsCompareResponse(List<IPlagiarismResult> plagiarismResults)
    {
        _plagiarismResults = plagiarismResults;
    }
    
    public UniDocumentsCompareResponse()
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