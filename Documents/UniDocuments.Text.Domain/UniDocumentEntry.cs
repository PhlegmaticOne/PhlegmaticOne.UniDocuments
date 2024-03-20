using UniDocuments.Text.Domain.Features;

namespace UniDocuments.Text.Domain;

public class UniDocumentEntry
{
    public UniDocument Comparing { get; }
    public UniDocument Original { get; }
    public UniDocumentFeaturesCollection SharedFeatures { get; }

    public UniDocumentEntry(UniDocument comparing, UniDocument original)
    {
        Comparing = comparing;
        Original = original;
        SharedFeatures = new UniDocumentFeaturesCollection();
    }

    public void TakeFeatures(UniDocumentFeatureContainer featureContainer)
    {
        if (featureContainer.HasCreatedFeatures == false)
        {
            return;
        }

        IUniDocumentFeaturesCollection features = featureContainer.BelongType switch
        {
            UniDocumentFeatureBelongType.Original => Original,
            UniDocumentFeatureBelongType.Comparing => Comparing,
            UniDocumentFeatureBelongType.Shared => SharedFeatures,
            _ => throw new ArgumentOutOfRangeException(nameof(featureContainer))
        };
        
        foreach (var createdFeature in featureContainer.CreatedFeatures!)
        {
            features.AddFeature(createdFeature);
        }
    }
}