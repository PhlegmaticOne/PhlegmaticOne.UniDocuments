using Newtonsoft.Json;

namespace UniDocuments.Text.Domain;

[Serializable]
public class UniDocument : IEquatable<UniDocument>
{
    public Guid Id { get; }
    public UniDocumentContent Content { get; }

    public static UniDocument FromString(string value)
    {
        return new UniDocument(Guid.Empty, UniDocumentContent.FromString(value));
    }
    
    public static UniDocument FromContent(UniDocumentContent value)
    {
        return new UniDocument(Guid.Empty, value);
    }
    
    [JsonConstructor]
    public UniDocument(Guid id, UniDocumentContent content)
    {
        Id = id;
        Content = content;
    }

    public string GetParagraph(int id)
    {
        return Content.Paragraphs[id];
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
