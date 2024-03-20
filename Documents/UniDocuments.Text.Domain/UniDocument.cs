using UniDocuments.Text.Domain.Features;

namespace UniDocuments.Text.Domain;

public class UniDocument : IUniDocumentFeaturesCollection
{
    private readonly UniDocumentFeaturesCollection _features;

    public Guid Id { get; }

    public static UniDocument Empty => new(Guid.Empty);

    public UniDocument(Guid id)
    {
        Id = id;
        _features = new UniDocumentFeaturesCollection();
    }

    public T GetFeature<T>()
    {
        return _features.GetFeature<T>();
    }

    public bool TryGetFeature<T>(out T? feature) where T : IUniDocumentFeature
    {
        return _features.TryGetFeature(out feature);
    }

    public void AddFeature(IUniDocumentFeature feature)
    {
        _features.AddFeature(feature);
    }

    public bool ContainsFeature(UniDocumentFeatureFlag featureFlag)
    {
        return _features.ContainsFeature(featureFlag);
    }
}
