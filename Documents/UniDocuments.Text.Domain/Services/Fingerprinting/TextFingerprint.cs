﻿using System.Diagnostics.CodeAnalysis;

namespace UniDocuments.Text.Domain.Services.Fingerprinting;

[SuppressMessage("ReSharper", "ConvertClosureToMethodGroup")]
[Serializable]
public class TextFingerprint
{
    public TextFingerprint(HashSet<uint> entries)
    {
        Entries = entries;
    }

    public static TextFingerprint FromBytes(byte[] bytes)
    {
        var decoded = new uint[bytes.Length / 4];
        Buffer.BlockCopy(bytes, 0, decoded, 0, bytes.Length);
        return new TextFingerprint(decoded.ToHashSet());
    }

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