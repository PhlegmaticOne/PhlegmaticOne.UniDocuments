using Microsoft.Data.SqlClient;

namespace UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;

public class DocumentLoadResponseFromStream : IDocumentLoadResponse, IAsyncDisposable
{
    private readonly Stream _stream;
    private readonly SqlTransaction? _transaction;

    public DocumentLoadResponseFromStream(Stream stream, Guid id, SqlTransaction? transaction = null)
    {
        Id = id;
        _stream = stream;
        _transaction = transaction;
    }
    
    public Guid Id { get; }
    public string Name { get; set; } = null!;
    
    public Stream ToStream()
    {
        return _stream;
    }

    public void Dispose()
    {
        _stream.Dispose();
        _transaction?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null) await _transaction.DisposeAsync();
    }
}