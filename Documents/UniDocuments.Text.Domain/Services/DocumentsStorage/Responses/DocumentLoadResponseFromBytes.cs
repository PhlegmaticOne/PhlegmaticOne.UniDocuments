namespace UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;

public class DocumentLoadResponseFromBytes : IDocumentLoadResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public byte[]? Bytes { get; set; }

    public Stream ToStream()
    {
        return new MemoryStream(Bytes!);
    }
}
