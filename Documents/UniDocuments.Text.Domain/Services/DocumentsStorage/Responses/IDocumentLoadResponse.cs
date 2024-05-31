namespace UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;

public interface IDocumentLoadResponse
{
    Guid Id { get; set; }
    string Name { get; set; }
    byte[]? Bytes { get; set; }
    Stream ToStream();
    
    static IDocumentLoadResponse NoContent()
    {
        return new DocumentLoadResponseFromBytes
        {
            Name = string.Empty,
            Bytes = null
        };
    }
}