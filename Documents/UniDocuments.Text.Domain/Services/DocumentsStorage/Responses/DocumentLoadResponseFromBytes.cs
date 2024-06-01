namespace UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;

public class DocumentLoadResponseFromBytes : IDocumentLoadResponse
{
    public Guid Id { get; init; }
    public string Name { get; set; } = null!;
    public byte[]? Bytes { get; init; }

    public Stream ToStream()
    {
        return new MemoryStream(Bytes!);
    }

    public void Dispose()
    {
        
    }
}
