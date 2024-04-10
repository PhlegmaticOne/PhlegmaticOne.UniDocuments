using Newtonsoft.Json;

namespace UniDocuments.Text.Domain.Services.Neural.Models;

public struct ParagraphSaveData
{
    [JsonProperty("o")] public int OriginalId;
    [JsonProperty("d")] public Guid DocumentId;
    [JsonIgnore] public string DocumentName;
}