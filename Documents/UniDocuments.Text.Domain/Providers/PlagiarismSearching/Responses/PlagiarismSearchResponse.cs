using Newtonsoft.Json;

namespace UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

[Serializable]
public class PlagiarismSearchResponse
{
    public PlagiarismSearchResponse(Guid documentId) : this(documentId, new List<ParagraphPlagiarismData>()) { }

    [JsonConstructor]
    public PlagiarismSearchResponse(Guid documentId, List<ParagraphPlagiarismData> suspiciousParagraphs)
    {
        DocumentId = documentId;
        SuspiciousParagraphs = suspiciousParagraphs;
    }

    [JsonProperty]
    public Guid DocumentId { get; }

    [JsonProperty]
    public List<ParagraphPlagiarismData> SuspiciousParagraphs { get; set; }
}