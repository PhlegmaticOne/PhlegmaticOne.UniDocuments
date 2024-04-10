using Newtonsoft.Json;

namespace UniDocuments.Text.Domain.Services.Neural.Models;

[Serializable]
public struct ParagraphNeuralSaveData
{
    [JsonProperty("o")] public int OriginalId;
    [JsonProperty("d")] public Guid DocumentId;
    [JsonIgnore] public string DocumentName;
}