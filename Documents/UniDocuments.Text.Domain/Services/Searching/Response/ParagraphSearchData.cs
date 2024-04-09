using Newtonsoft.Json;

namespace UniDocuments.Text.Domain.Services.Searching.Response;

[Serializable]
public struct ParagraphSearchData
{
    [JsonProperty]
    public int OriginalId;
    [JsonProperty]
    public Guid DocumentId;
    [JsonProperty]
    public string DocumentName;
}