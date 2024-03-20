namespace UniDocuments.Text.Domain.Features;

public class UniDocumentFeaturesCollection : IUniDocumentFeaturesCollection
{
    private readonly Dictionary<Type, IUniDocumentFeature> _features;
    
    public UniDocumentFeaturesCollection()
    {
        _features = new Dictionary<Type, IUniDocumentFeature>();
    }
    
    public T GetFeature<T>()
    {
        return (T)_features[typeof(T)];
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
        return _features.Any(x => x.Value.FeatureFlag.Equals(featureFlag));
    }
}