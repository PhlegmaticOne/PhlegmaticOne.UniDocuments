using Microsoft.ML;
using PhlegmaticOne.UniDocuments.Documents.Core;
using PhlegmaticOne.UniDocuments.Documents.Core.Algorithms;
using PhlegmaticOne.UniDocuments.Documents.Core.Features.Content;

namespace PhlegmaticOne.UniDocuments.Documents.Algorithms.Cosine;

public class PlagiarismAlgorithmCosine : IPlagiarismAlgorithm
{
    public IPlagiarismResult Perform(UniDocument comparing, UniDocument original)
    {
        if (!comparing.TryGetFeature<UniDocumentFeatureContent>(out var comparingContent) ||
            !original.TryGetFeature<UniDocumentFeatureContent>(out var originalContent))
        {
            return PlagiarismResultError.FromMessage("No content provided");
        }

        var ml = new MLContext();

        return new PlagiarismResultCosine(1);
    }
}
