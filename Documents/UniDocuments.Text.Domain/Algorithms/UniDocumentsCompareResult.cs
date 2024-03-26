using Newtonsoft.Json;

namespace UniDocuments.Text.Domain.Algorithms;

[Serializable]
public class UniDocumentsCompareResult
{
    [JsonProperty("result")]
    private List<IPlagiarismResult> _plagiarismResults;
    
    [JsonConstructor]
    public UniDocumentsCompareResult(List<IPlagiarismResult> plagiarismResults)
    {
        _plagiarismResults = plagiarismResults;
    }
    
    public UniDocumentsCompareResult()
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