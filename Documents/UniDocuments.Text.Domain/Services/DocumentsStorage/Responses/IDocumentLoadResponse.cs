namespace UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;

public interface IDocumentLoadResponse
{
    Guid Id { get; }
    string Name { get; set; }
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