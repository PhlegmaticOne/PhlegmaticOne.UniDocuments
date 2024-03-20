using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Algorithms;
using UniDocuments.Text.Domain.Features;
using UniDocuments.Text.Features.TextVector;
using UniDocuments.Text.Math;
using UniDocuments.Text.Plagiarism.TsSs.Data;

namespace UniDocuments.Text.Plagiarism.TsSs.Algorithm;

public class PlagiarismAlgorithmTsSs : IPlagiarismAlgorithm
{
    public PlagiarismAlgorithmFeatureFlag FeatureFlag => PlagiarismAlgorithmTsSsFeatureFlag.Instance;

    public IPlagiarismResult Perform(UniDocumentEntry entry)
    {
        if (!entry.SharedFeatures.TryGetFeature<UniDocumentFeatureTextVector>(out var textVector))
        {
            return PlagiarismResultTsSs.Error;
        }

        var originalVector = textVector!.OriginalVector;
        var comparingVector = textVector.ComparingVector;
        var metric = CalculateTsSs(originalVector, comparingVector);
        return PlagiarismResultTsSs.FromTsSs(metric);
    }

    public IEnumerable<UniDocumentFeatureFlag> GetRequiredFeatures()
    {
        yield return UniDocumentFeatureTextVectorFlag.Instance;
    }

    private static double CalculateTsSs(UniVector<int> original, UniVector<int> comparing)
    {
        var theta = ThetaAngle(original, comparing);
        var normOriginal = original.Norm();
        var normComparing = comparing.Norm();
        
        var distance = original.EuclideanDistance(comparing);
        var magnitudeDifference = (normComparing - normOriginal).Abs();
        var ss = theta.SegmentSquare(distance + magnitudeDifference);
        var ts = theta.TriangleSquare(normOriginal, normComparing);

        return ts * ss;
    }

    private static UniAngle ThetaAngle(UniVector<int> original, UniVector<int> comparing)
    {
        var cosine = original.Cosine(comparing);
        return UniAngle.ArcCos(cosine) + UniAngle.FromDegrees(10);
    }
}