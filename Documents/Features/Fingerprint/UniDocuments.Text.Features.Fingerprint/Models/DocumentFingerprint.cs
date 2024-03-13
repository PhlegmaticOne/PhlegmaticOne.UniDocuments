using System.Collections;

namespace UniDocuments.Text.Features.Fingerprint.Models;

public class DocumentFingerprint : IEnumerable<uint>
{
    public DocumentFingerprint(HashSet<uint> entries)
    {
        Entries = entries;
    }

    public HashSet<uint> Entries { get; set; }

    public bool HasFingerprint(uint fingerprint)
    {
        return Entries.Contains(fingerprint);
    }
    
    public IEnumerator<uint> GetEnumerator()
    {
        return Entries.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)this).GetEnumerator();
    }
}