using System.Collections;

namespace UniDocuments.Text.Plagiarism.Winnowing.Data;

public class Fingerprint : IEnumerable<uint>
{
    public Fingerprint(HashSet<uint> entries)
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