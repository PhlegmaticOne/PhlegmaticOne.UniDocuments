using Newtonsoft.Json;

namespace UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

[Serializable]
public struct DocumentSearchData
{
    [JsonProperty] public Guid Id;
    [JsonProperty] public double Similarity;

    public DocumentSearchData(Guid id, double similarity)
    {
        Id = id;
        Similarity = similarity;
    }
}