using System.Diagnostics.CodeAnalysis;

namespace UniDocuments.Text.Domain.Services.Fingerprinting;

[SuppressMessage("ReSharper", "ConvertClosureToMethodGroup")]
[Serializable]
public class TextFingerprint
{
    public TextFingerprint(Guid documentId, HashSet<uint> entries)
    {
        DocumentId = documentId;
        Entries = entries;
    }

    public Guid DocumentId { get; }
    public HashSet<uint> Entries { get; set; }

    public int GetSharedPrintsCount(TextFingerprint other)
    {
        var copy = Entries.ToHashSet();
        copy.IntersectWith(other.Entries);
        return copy.Count;
    }

    public double CalculateJaccard(TextFingerprint other)
    {
        var otherEntries = other.Entries;
        var intersectionSize = Entries.Count(finger => otherEntries.Contains(finger));
        var unionSize = Entries.Count + other.Entries.Count - intersectionSize;
        var similarity = (double)intersectionSize / unionSize;
        return similarity;
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