using UniDocuments.Text.Plagiarism.Algorithms.Core;
using UniDocuments.Text.Plagiarism.Matching.Data.Models;

namespace UniDocuments.Text.Plagiarism.Matching.Data;

[Serializable]
public class PlagiarismResultMatching : IPlagiarismResult
{
    public PlagiarismResultMatching(List<MatchEntry> matches, bool isSucceed)
    {
        Matches = matches;
        IsSucceed = isSucceed;
    }
    
    public static PlagiarismResultMatching Error => new(new List<MatchEntry>(), false);
    public static PlagiarismResultMatching FromBlocks(List<MatchEntry> blocks) => new(blocks, true);
    public List<MatchEntry> Matches { get; }
    public bool IsSucceed { get; }
    
    public override string ToString()
    {
        return $"Matches: {Matches.Count}";
    }
}