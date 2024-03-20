namespace UniDocuments.Text.Domain.Algorithms;

public class PlagiarismAlgorithmFeatureFlag : IEquatable<PlagiarismAlgorithmFeatureFlag>
{
    protected PlagiarismAlgorithmFeatureFlag() { }
    public PlagiarismAlgorithmFeatureFlag(string value)
    {
        Value = value;
    }

    public virtual string Value { get; } = null!;
    
    public bool Equals(PlagiarismAlgorithmFeatureFlag? other)
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
        return Equals((PlagiarismAlgorithmFeatureFlag)obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value;
    }
}