using Newtonsoft.Json;

namespace UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

[Serializable]
public class PlagiarismSearchResponseText
{
    [JsonConstructor]
    public PlagiarismSearchResponseText(List<ParagraphPlagiarismData> suspiciousParagraphs)
    {
        SuspiciousParagraphs = suspiciousParagraphs;
    }

    [JsonProperty]
    public List<ParagraphPlagiarismData> SuspiciousParagraphs { get; set; }
}