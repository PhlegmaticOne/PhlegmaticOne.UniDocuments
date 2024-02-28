using PhlegmaticOne.UniDocuments.Documents.Core.Features;

namespace PhlegmaticOne.UniDocuments.Documents.Core;

public class UniDocument
{
    private readonly Dictionary<Type, IUniDocumentFeature> _features;

    public Guid Id { get; }

    public UniDocument(Guid id)
    {
        Id = id;
        _features = new Dictionary<Type, IUniDocumentFeature>();
    }

    public bool TryGetFeature<T>(out T? feature) where T : IUniDocumentFeature
    {
        feature = default;

        if(_features.TryGetValue(typeof(T), out var baseFeature))
        {
            feature = (T)baseFeature;
            return true;
        }

        return false;
    }

    public void AddFeature(IUniDocumentFeature feature)
    {
        _features.TryAdd(feature.GetType(), feature);
    }

    public bool ContainsFeature(UniDocumentFeatureFlag featureFlag)
    {
        return _features.Any(x => x.Value.FeatureFlag == featureFlag);
    }
}
