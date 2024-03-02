using UniDocuments.Text.Core.Features;

namespace UniDocuments.Text.Core;

public class UniDocument
{
    private readonly Dictionary<Type, IUniDocumentFeature> _features;

    public Guid Id { get; }

    public static UniDocument Empty => new(Guid.Empty);

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

    public UniDocument AddFeature<T>(T feature) where T : IUniDocumentFeature
    {
        _features.TryAdd(typeof(T), feature);
        return this;
    }

    public bool ContainsFeature(UniDocumentFeatureFlag featureFlag)
    {
        return _features.Any(x => x.Value.FeatureFlag == featureFlag);
    }
}
