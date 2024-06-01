using UniDocuments.Text.Domain.Extensions;

namespace UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;

public class DocumentLoadResponseFromStream : IDocumentLoadResponse
{
    private readonly Stream _stream;

    public DocumentLoadResponseFromStream(Stream stream, Guid id)
    {
        Id = id;
        _stream = stream;
    }
    
    public Guid Id { get; }
    public string Name { get; set; } = null!;
    
    public Stream ToStream()
    {
        _stream.SeekToZero();
        return _stream;
    }

    public void Dispose()
    {
        _stream.Dispose();
    }
}