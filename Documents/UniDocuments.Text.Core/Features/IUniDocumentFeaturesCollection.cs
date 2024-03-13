namespace UniDocuments.Text.Core.Features;

public interface IUniDocumentFeaturesCollection
{
    T GetFeature<T>();
    bool TryGetFeature<T>(out T? feature) where T : IUniDocumentFeature;
    void AddFeature(IUniDocumentFeature feature);
    bool ContainsFeature(UniDocumentFeatureFlag featureFlag);
}