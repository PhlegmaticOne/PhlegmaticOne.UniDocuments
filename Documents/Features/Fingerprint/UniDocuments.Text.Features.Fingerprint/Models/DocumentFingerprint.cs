using System.Diagnostics.CodeAnalysis;

namespace UniDocuments.Text.Features.Fingerprint.Models;

[SuppressMessage("ReSharper", "ConvertClosureToMethodGroup")]
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

    public bool HasFingerprint(uint fingerprint)
    {
        return Entries.Contains(fingerprint);
    }

    public byte[] ToByteArray()
    {
        return Entries.SelectMany(o => BitConverter.GetBytes(o)).ToArray();
    }
}