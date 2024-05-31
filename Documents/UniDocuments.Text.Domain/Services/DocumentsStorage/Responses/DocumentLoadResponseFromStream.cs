using UniDocuments.Text.Domain.Extensions;

namespace UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;

public class DocumentLoadResponseFromStream : IDocumentLoadResponse
{
    private readonly Stream _stream;

    public DocumentLoadResponseFromStream(Stream stream)
    {
        _stream = stream;
    }
    
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public byte[]? Bytes
    {
        get => LoadBytes();
        set { }
    }

    public Stream ToStream()
    {
        return _stream;
    }

    private byte[] LoadBytes()
    {
        using var memoryStream = new MemoryStream();
        _stream.SeekToZero();
        _stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}