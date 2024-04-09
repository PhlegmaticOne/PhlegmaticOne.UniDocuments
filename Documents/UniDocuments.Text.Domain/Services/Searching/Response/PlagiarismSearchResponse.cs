using Newtonsoft.Json;

namespace UniDocuments.Text.Domain.Services.Searching.Response;

[Serializable]
public class PlagiarismSearchResponse
{
    [JsonConstructor]
    public PlagiarismSearchResponse(List<ParagraphPlagiarismData> suspiciousParagraphs, List<DocumentSearchData> suspiciousDocuments)
    {
        SuspiciousParagraphs = suspiciousParagraphs;
        SuspiciousDocuments = suspiciousDocuments;
    }

    [JsonProperty]
    public List<ParagraphPlagiarismData> SuspiciousParagraphs { get; set; }
    [JsonProperty]
    public List<DocumentSearchData> SuspiciousDocuments { get; set; }
}