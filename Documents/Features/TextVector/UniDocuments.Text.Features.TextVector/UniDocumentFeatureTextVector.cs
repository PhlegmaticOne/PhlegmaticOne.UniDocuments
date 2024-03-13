using UniDocuments.Text.Core.Features;
using UniDocuments.Text.Math;

namespace UniDocuments.Text.Features.TextVector;

public class UniDocumentFeatureTextVector : IUniDocumentFeature
{
    public UniDocumentFeatureTextVector(UniVector<int> originalVector, UniVector<int> comparingVector)
    {
        OriginalVector = originalVector;
        ComparingVector = comparingVector;
    }

    public UniVector<int> OriginalVector { get; }
    public UniVector<int> ComparingVector { get; }
    public UniDocumentFeatureFlag FeatureFlag => UniDocumentFeatureTextVectorFlag.Instance;
}