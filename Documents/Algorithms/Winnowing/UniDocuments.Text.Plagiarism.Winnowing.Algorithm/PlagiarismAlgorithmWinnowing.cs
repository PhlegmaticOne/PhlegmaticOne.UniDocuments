using UniDocuments.Text.Core;
using UniDocuments.Text.Core.Algorithms;
using UniDocuments.Text.Plagiarism.Winnowing.Data;

namespace UniDocuments.Text.Plagiarism.Winnowing.Algorithm;

public class PlagiarismAlgorithmWinnowing : PlagiarismAlgorithm<PlagiarismResultWinnowing>
{    
    public override PlagiarismResultWinnowing PerformExact(UniDocument original, UniDocument comparing)
    {
        return new PlagiarismResultWinnowing(new Fingerprint(new HashSet<uint>()), true);
    }
}