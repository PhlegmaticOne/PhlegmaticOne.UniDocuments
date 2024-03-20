using UniDocuments.Text.Domain.Algorithms;

namespace UniDocuments.Text.Plagiarism.Cosine.Algorithm;

public class PlagiarismAlgorithmCosineFeatureFlag : PlagiarismAlgorithmFeatureFlag
{
    public static PlagiarismAlgorithmFeatureFlag Instance => new PlagiarismAlgorithmCosineFeatureFlag();
    public override string Value => "CosineSimilarity";
}