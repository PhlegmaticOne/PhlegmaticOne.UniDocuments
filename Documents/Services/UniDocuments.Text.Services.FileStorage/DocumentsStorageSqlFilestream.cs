using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlTypes;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Requests;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;
using UniDocuments.Text.Services.FileStorage.Connection;

namespace UniDocuments.Text.Services.FileStorage;

public class DocumentsStorageSqlFilestream : IDocumentsStorage
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DocumentsStorageSqlFilestream(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<IDocumentLoadResponse?> LoadAsync(Guid id, CancellationToken cancellationToken)
    {
        await using var sqlConnection = _dbConnectionFactory.CreateConnection();
        await using var command = FileSqlCommands.CreateSelectFileCommand(sqlConnection, id);
        await using var transaction = sqlConnection.BeginTransaction();
        command.Transaction = transaction;
        
        var transactionContext = Array.Empty<byte>();
        var savedFileName = string.Empty;
        var path = string.Empty;

        await using (var reader = await command.ExecuteReaderAsync(cancellationToken))
        {
            while (await reader.ReadAsync(cancellationToken))
            {
                transactionContext = ReadTransactionContext(reader);
                path = ReadFilePath(reader);
                savedFileName = ReadFileName(reader);
            }
        }

        var stream = new SqlFileStream(path, transactionContext, FileAccess.Read);
            
        return new DocumentLoadResponseFromStream(stream)
        {
            Id = id,
            Name = savedFileName
        };
    }

    public async IAsyncEnumerable<IDocumentLoadResponse> LoadAsync(IList<Guid> ids)
    {
        await using var connection = _dbConnectionFactory.CreateConnection();
        await using var sqlConnection = _dbConnectionFactory.CreateConnection();
        await using var command = FileSqlCommands.CreateSelectFilesCommand(sqlConnection, ids);
        await using var transaction = sqlConnection.BeginTransaction();
        command.Transaction = transaction;

        await using var reader = await command.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            var transactionContext = ReadTransactionContext(reader);
            var path = ReadFilePath(reader);
            var savedFileName = ReadFileName(reader);
            var id = ReadFileId(reader);
            var stream = new SqlFileStream(path, transactionContext, FileAccess.Read);

            yield return new DocumentLoadResponseFromStream(stream)
            {
                Id = id,
                Name = savedFileName
            };
        }
    }

    public async Task<Guid> SaveAsync(StorageSaveRequest saveRequest, CancellationToken cancellationToken)
    {
        await using var sqlConnection = _dbConnectionFactory.CreateConnection();
        var fileName = saveRequest.Name;
        var transactionContext = Array.Empty<byte>();
        var path = string.Empty;

        await using var command = FileSqlCommands.CreateInsertFileCommand(sqlConnection, fileName, saveRequest.Id);
        await using var transaction = sqlConnection.BeginTransaction();
        command.Transaction = transaction;

        await using (var reader = await command.ExecuteReaderAsync(cancellationToken))
        {
            while (await reader.ReadAsync(cancellationToken))
            {
                transactionContext = ReadTransactionContext(reader);
                path = ReadFilePath(reader);
            }
        }

        await using (var destination = new SqlFileStream(path, transactionContext, FileAccess.Write))
        {
            await using var source = saveRequest.Stream;
            await source.CopyToAsync(destination, cancellationToken);
        }

        await command.Transaction.CommitAsync(cancellationToken);
        return saveRequest.Id;
    }
    
    private static byte[] ReadTransactionContext(SqlDataReader reader) => (byte[])reader["transactionContext"];
    private static string ReadFilePath(SqlDataReader reader) => reader["filePath"].ToString()!;
    private static string ReadFileName(SqlDataReader reader) => reader["FileName"].ToString()!;
    private static Guid ReadFileId(SqlDataReader reader) => Guid.Parse(reader["id"].ToString()!);
}