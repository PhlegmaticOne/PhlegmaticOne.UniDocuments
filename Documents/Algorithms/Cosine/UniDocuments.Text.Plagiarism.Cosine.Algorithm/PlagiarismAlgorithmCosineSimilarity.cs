using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Algorithms;
using UniDocuments.Text.Domain.Features;
using UniDocuments.Text.Features.TextVector;
using UniDocuments.Text.Math;
using UniDocuments.Text.Plagiarism.Cosine.Data;

namespace UniDocuments.Text.Plagiarism.Cosine.Algorithm;

public class PlagiarismAlgorithmCosineSimilarity : PlagiarismAlgorithm<PlagiarismResultCosine>
{
    public override PlagiarismAlgorithmFeatureFlag FeatureFlag => PlagiarismAlgorithmCosineFeatureFlag.Instance;

    public override IEnumerable<UniDocumentFeatureFlag> GetRequiredFeatures()
    {
        yield return UniDocumentFeatureTextVectorFlag.Instance;
    }

    public override PlagiarismResultCosine PerformExact(UniDocumentEntry entry)
    {
        if (!entry.SharedFeatures.TryGetFeature<UniDocumentFeatureTextVector>(out var textVector))
        {
            return PlagiarismResultCosine.Error;
        }

        var originalVector = textVector!.OriginalVector;
        var comparingVector = textVector.ComparingVector;
        var metric = originalVector.Cosine(comparingVector);
        return PlagiarismResultCosine.FromCosine(metric);
    }
}
