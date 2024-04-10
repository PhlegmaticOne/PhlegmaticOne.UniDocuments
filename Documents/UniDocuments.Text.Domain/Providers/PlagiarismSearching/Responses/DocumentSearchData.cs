using Newtonsoft.Json;

namespace UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

[Serializable]
public struct DocumentSearchData
{
    [JsonProperty] public Guid Id;
    [JsonProperty] public string Name;
}