using Newtonsoft.Json;

namespace UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

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