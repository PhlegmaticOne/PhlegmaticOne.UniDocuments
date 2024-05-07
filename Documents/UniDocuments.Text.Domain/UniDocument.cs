using Newtonsoft.Json;

namespace UniDocuments.Text.Domain;

[Serializable]
public class UniDocument : IEquatable<UniDocument>
{
    public Guid Id { get; }
    public string Name { get; }
    public UniDocumentContent Content { get; }

    public static UniDocument FromString(string value, string? name = null)
    {
        var resultName = name ?? GenerateRandomName();
        return new UniDocument(Guid.Empty, UniDocumentContent.FromString(value), resultName);
    }
    
    public static UniDocument FromContent(UniDocumentContent value, string? name = null)
    {
        var resultName = name ?? GenerateRandomName();
        return new UniDocument(Guid.Empty, value, resultName);
    }
    
    [JsonConstructor]
    public UniDocument(Guid id, UniDocumentContent content, string name)
    {
        Id = id;
        Content = content;
        Name = name;
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

    private static string GenerateRandomName()
    {
        return $"unknown_{Guid.NewGuid()}";
    }
}
