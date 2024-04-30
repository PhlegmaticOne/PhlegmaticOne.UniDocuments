using Newtonsoft.Json;

namespace UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

[Serializable]
public class PlagiarismSearchResponse
{
    public PlagiarismSearchResponse() : this(new List<ParagraphPlagiarismData>(), Array.Empty<DocumentSearchData>()) { }
    
    [JsonConstructor]
    public PlagiarismSearchResponse(List<ParagraphPlagiarismData> suspiciousParagraphs, DocumentSearchData[] suspiciousDocuments)
    {
        SuspiciousParagraphs = suspiciousParagraphs;
        SuspiciousDocuments = suspiciousDocuments;
    }

    [JsonProperty]
    public List<ParagraphPlagiarismData> SuspiciousParagraphs { get; set; }
    [JsonProperty]
    public DocumentSearchData[] SuspiciousDocuments { get; set; }
}