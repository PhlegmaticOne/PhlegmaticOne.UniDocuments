using UniDocuments.Text.Plagiarism.Algorithms.Core;

namespace UniDocuments.Text.Plagiarism.Matching.Algorithm;

public class PlagiarismAlgorithmMatchingFeatureFlag : PlagiarismAlgorithmFeatureFlag
{
    public static PlagiarismAlgorithmFeatureFlag Instance => new PlagiarismAlgorithmMatchingFeatureFlag();
    public override string Value => "Matching";
}