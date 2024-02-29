namespace PhlegmaticOne.UniDocuments.Documents.Features.Metrics.Models;

public class FingerprintEntry : IEquatable<FingerprintEntry>
{
    public FingerprintEntry(int hash, int position, int length)
    {
        Hash = hash;
        Position = position;
        Length = length;
    }

    public int Hash { get; }
    public int Position { get; }
    public int Length { get; }

    public override bool Equals(object? obj)
    {
        return obj is FingerprintEntry entry && Equals(entry);
    }

    public override int GetHashCode()
    {
        return Hash.GetHashCode();
    }

    public bool Equals(FingerprintEntry? other)
    {
        return other is not null && other.Hash == Hash;
    }
}
