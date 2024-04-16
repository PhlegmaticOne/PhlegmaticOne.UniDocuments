using UniDocuments.Text.Domain.Algorithms;

namespace UniDocuments.Text.Plagiarism.TsSs.Algorithm;

public class PlagiarismAlgorithmTsSsFeatureFlag : PlagiarismAlgorithmFeatureFlag
{
    public static PlagiarismAlgorithmFeatureFlag Instance => new PlagiarismAlgorithmTsSsFeatureFlag();
    public override string Value => "tsss";
}