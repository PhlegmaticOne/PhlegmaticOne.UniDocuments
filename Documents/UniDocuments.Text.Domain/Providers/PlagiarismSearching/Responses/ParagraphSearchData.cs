using Newtonsoft.Json;

namespace UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

[Serializable]
public struct ParagraphSearchData
{
    [JsonProperty] public int OriginalId;
    [JsonProperty] public Guid DocumentId;
    [JsonProperty] public float Similarity;
    [JsonProperty] public string DocumentName;
}