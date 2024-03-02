namespace UniDocuments.Text.Plagiarism.Matching.Algorithm.Grams;

internal readonly struct GramEntry
{
    public readonly string Value;
    public readonly int Index;
    public readonly int Length;

    public GramEntry(string value, int index, int length)
    {
        Value = value;
        Index = index;
        Length = length;
    }

    public override string ToString()
    {
        return Value.ToLower();
    }

    public bool Equals(GramEntry? other)
    {
        return other.HasValue && Value.Equals(other.Value.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((GramEntry)obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
    }
}