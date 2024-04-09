using Newtonsoft.Json;

namespace UniDocuments.Text.Domain.Services.Neural;

public struct ParagraphSaveData
{
    [JsonProperty("o")]
    public int OriginalId;
    [JsonProperty("d")]
    public Guid DocumentId;
}