using Newtonsoft.Json;

namespace UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

[Serializable]
public class PlagiarismSearchResponseDocument
{
    public PlagiarismSearchResponseDocument() : this(new List<ParagraphPlagiarismData>(), Array.Empty<DocumentSearchData>()) { }
    
    [JsonConstructor]
    public PlagiarismSearchResponseDocument(List<ParagraphPlagiarismData> suspiciousParagraphs, DocumentSearchData[] suspiciousDocuments)
    {
        SuspiciousParagraphs = suspiciousParagraphs;
        SuspiciousDocuments = suspiciousDocuments;
    }

    [JsonProperty]
    public List<ParagraphPlagiarismData> SuspiciousParagraphs { get; set; }
    [JsonProperty]
    public DocumentSearchData[] SuspiciousDocuments { get; set; }
}