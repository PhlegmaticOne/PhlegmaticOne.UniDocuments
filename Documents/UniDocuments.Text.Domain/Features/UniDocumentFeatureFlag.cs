namespace UniDocuments.Text.Domain.Features;

public class UniDocumentFeatureFlag : IEquatable<UniDocumentFeatureFlag>
{
    protected UniDocumentFeatureFlag() { }
    public UniDocumentFeatureFlag(string value)
    {
        Value = value;
    }

    public virtual string Value { get; } = null!;
    public virtual bool IsShared => false;
    public virtual int SetupOrder => 0;
    public virtual IEnumerable<UniDocumentFeatureFlag> RequiredFeatures => Enumerable.Empty<UniDocumentFeatureFlag>();
    
    public bool Equals(UniDocumentFeatureFlag? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((UniDocumentFeatureFlag)obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}