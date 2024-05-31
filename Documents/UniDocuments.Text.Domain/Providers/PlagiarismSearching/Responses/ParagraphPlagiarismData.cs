using Newtonsoft.Json;

namespace UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

[Serializable]
public class ParagraphPlagiarismData
{
    [JsonConstructor]
    public ParagraphPlagiarismData(int paragraphId, List<ParagraphSearchData> suspiciousParagraphs)
    {
        ParagraphId = paragraphId;
        SuspiciousParagraphs = suspiciousParagraphs;
    }

    public int ParagraphId { get; set; }
    [JsonProperty]
    public List<ParagraphSearchData> SuspiciousParagraphs { get; set; }

    public HashSet<Guid> SelectUniqueIds()
    {
        return SuspiciousParagraphs.Select(x => x.DocumentId).ToHashSet();
    }
}