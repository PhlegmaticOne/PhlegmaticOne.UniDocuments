using Newtonsoft.Json;

namespace UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

[Serializable]
public class PlagiarismSearchResponseDocument
{
    public PlagiarismSearchResponseDocument(Guid documentId, string documentName) :
        this(documentId, documentName, new List<ParagraphPlagiarismData>())
    {
    }

    [JsonConstructor]
    public PlagiarismSearchResponseDocument(Guid documentId, string documentName, List<ParagraphPlagiarismData> suspiciousParagraphs)
    {
        DocumentId = documentId;
        DocumentName = documentName;
        SuspiciousParagraphs = suspiciousParagraphs;
    }

    [JsonProperty]
    public Guid DocumentId { get; }

    [JsonProperty]
    public string DocumentName { get; }

    [JsonProperty]
    public List<ParagraphPlagiarismData> SuspiciousParagraphs { get; set; }
}