using Newtonsoft.Json;

namespace UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

[Serializable]
public struct ParagraphSearchData
{
    [JsonProperty] public int Id;
    [JsonProperty] public Guid DocumentId;
    [JsonProperty] public double Similarity;
}