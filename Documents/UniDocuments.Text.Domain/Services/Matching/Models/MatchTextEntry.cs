using Newtonsoft.Json;

namespace UniDocuments.Text.Domain.Services.Matching.Models;

[Serializable]
public class MatchTextEntry
{
    [JsonProperty("ssi")]
    public int SourceFragmentStartIndex { get; }
    [JsonProperty("sl")]
    public int SourceFragmentLength { get; }
    [JsonProperty("msi")]
    public int MatchedFragmentStartIndex { get; }
    [JsonProperty("ml")]
    public int MatchedFragmentLength { get; }
    
    [JsonConstructor]
    public MatchTextEntry(
        int sourceFragmentStartIndex, int sourceFragmentEndIndex, 
        int matchedFragmentStartIndex, int matchedFragmentEndIndex)
    {
        SourceFragmentStartIndex = sourceFragmentStartIndex;
        SourceFragmentLength = sourceFragmentEndIndex - sourceFragmentStartIndex + 1;
        MatchedFragmentStartIndex = matchedFragmentStartIndex;
        MatchedFragmentLength = matchedFragmentEndIndex - matchedFragmentStartIndex + 1;
    }
}