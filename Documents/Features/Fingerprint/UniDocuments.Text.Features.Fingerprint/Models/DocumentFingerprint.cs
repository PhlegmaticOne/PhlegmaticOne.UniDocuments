using System.Diagnostics.CodeAnalysis;

namespace UniDocuments.Text.Features.Fingerprint.Models;

[SuppressMessage("ReSharper", "ConvertClosureToMethodGroup")]
[Serializable]
public class DocumentFingerprint
{
    public DocumentFingerprint(HashSet<uint> entries)
    {
        Entries = entries;
    }

    public static DocumentFingerprint FromBytes(byte[] bytes)
    {
        var decoded = new uint[bytes.Length / 4];
        Buffer.BlockCopy(bytes, 0, decoded, 0, bytes.Length);
        return new DocumentFingerprint(decoded.ToHashSet());
    }

    public HashSet<uint> Entries { get; set; }

    public int GetSharedPrintsCount(DocumentFingerprint other)
    {
        var copy = Entries.ToHashSet();
        copy.IntersectWith(other.Entries);
        return copy.Count;
    }

    public double CalculateJaccard(DocumentFingerprint other)
    {
        var intersection = Entries.ToHashSet();
        var union = Entries.ToHashSet();
        intersection.IntersectWith(other.Entries);
        union.UnionWith(other.Entries);
        return (double)intersection.Count / union.Count;
    }

    public bool HasFingerprint(uint fingerprint)
    {
        return Entries.Contains(fingerprint);
    }

    public byte[] ToByteArray()
    {
        return Entries.SelectMany(o => BitConverter.GetBytes(o)).ToArray();
    }
}