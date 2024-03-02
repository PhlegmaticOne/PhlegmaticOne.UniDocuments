namespace UniDocuments.Text.Plagiarism.Matching.Data.Models;

[Serializable]
public struct MatchEntry
{
    public int SourceFragmentStartIndex;
    public int SourceFragmentLength;
    public int MatchedFragmentStartIndex;
    public int MatchedFragmentLength;
    
    public MatchEntry(
        int sourceFragmentStartIndex, int sourceFragmentEndIndex, 
        int matchedFragmentStartIndex, int matchedFragmentEndIndex)
    {
        SourceFragmentStartIndex = sourceFragmentStartIndex;
        SourceFragmentLength = sourceFragmentEndIndex - sourceFragmentStartIndex + 1;
        MatchedFragmentStartIndex = matchedFragmentStartIndex;
        MatchedFragmentLength = matchedFragmentEndIndex - matchedFragmentStartIndex + 1;
    }
}