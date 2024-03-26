using UniDocuments.Text.Domain.Features;

namespace UniDocuments.Text.Domain;

public class UniDocument : IEquatable<UniDocument>, IUniDocumentFeaturesCollection
{
    private readonly UniDocumentFeaturesCollection _features;

    public Guid Id { get; }

    public static UniDocument Empty => new(Guid.Empty);

    public UniDocument(Guid id)
    {
        Id = id;
        _features = new UniDocumentFeaturesCollection();
    }
    
    public UniDocument(Guid id, params IUniDocumentFeature[] startFeatures) : this(id)
    {
        foreach (var startFeature in startFeatures)
        {
            AddFeature(startFeature);
        }
    }

    public UniDocument WithFeature(IUniDocumentFeature feature)
    {
        AddFeature(feature);
        return this;
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

    public bool Equals(UniDocument? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((UniDocument)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
