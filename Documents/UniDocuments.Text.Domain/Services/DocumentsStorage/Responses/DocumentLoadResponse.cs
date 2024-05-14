namespace UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;

public class DocumentLoadResponse
{
    public string Name { get; set; } = null!;
    public byte[]? Bytes { get; set; }
    
    public static DocumentLoadResponse NoContent()
    {
        return new DocumentLoadResponse
        {
            Name = string.Empty,
            Bytes = null
        };
    }

    public Stream ToStream()
    {
        return new MemoryStream(Bytes!);
    }
}
