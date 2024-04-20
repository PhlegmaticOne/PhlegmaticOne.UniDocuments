namespace UniDocuments.Text.Domain;

public class UniDocument : IEquatable<UniDocument>
{
    public Guid Id { get; }
    public UniDocumentContent Content { get; }

    public static UniDocument Empty => new(Guid.Empty);
    
    public UniDocument(Guid id, UniDocumentContent? content = null)
    {
        Id = id;
        Content = content ?? UniDocumentContent.FromString(string.Empty);
    }

    public bool Equals(UniDocument? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((UniDocument)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
