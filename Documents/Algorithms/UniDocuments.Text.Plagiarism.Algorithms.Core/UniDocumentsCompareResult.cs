namespace UniDocuments.Text.Plagiarism.Algorithms.Core;

[Serializable]
public class UniDocumentsCompareResult
{
    private readonly List<IPlagiarismResult> _plagiarismResults;
    
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