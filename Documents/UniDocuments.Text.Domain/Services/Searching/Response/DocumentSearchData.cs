using Newtonsoft.Json;

namespace UniDocuments.Text.Domain.Services.Searching.Response;

[Serializable]
public struct DocumentSearchData
{
    [JsonProperty]
    public Guid Id;
    [JsonProperty]
    public string Name;
}