using Newtonsoft.Json;

namespace UniDocuments.Text.Domain;

[Serializable]
public class UniDocument : IEquatable<UniDocument>
{
    public Guid Id { get; }
    public UniDocumentContent? Content { get; }

    public static UniDocument Empty => new(Guid.Empty);
    
    public bool HasData => Content is not null;
    
    [JsonConstructor]
    public UniDocument(Guid id, UniDocumentContent? content = null)
    {
        Id = id;
        Content = content;
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
        return obj.GetType() == GetType() && Equals((UniDocument)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return Id.ToString();
    }
}
