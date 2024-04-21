using Newtonsoft.Json;

namespace UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

[Serializable]
public struct DocumentSearchData
{
    [JsonProperty] public Guid Id;
    [JsonProperty] public string Name;
    [JsonProperty] public double Similarity;

    public DocumentSearchData(Guid id, string name, double similarity)
    {
        Id = id;
        Name = name;
        Similarity = similarity;
    }
}