using System.Data;
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
        await using var transaction = sqlConnection.BeginTransaction(IsolationLevel.ReadCommitted);
        command.Transaction = transaction;
        
        var transactionContext = Array.Empty<byte>();
        var path = string.Empty;

        await using (var reader = await command.ExecuteReaderAsync(cancellationToken))
        {
            while (await reader.ReadAsync(cancellationToken))
            {
                transactionContext = ReadTransactionContext(reader);
                path = ReadFilePath(reader);
            }
        }

        if (string.IsNullOrEmpty(path))
        {
            await transaction.CommitAsync(cancellationToken);
            return IDocumentLoadResponse.NoContent();
        }

        var stream = new SqlFileStream(path, transactionContext, FileAccess.Read);
        return new DocumentLoadResponseFromStream(stream, id, transaction);
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
            var id = ReadFileId(reader);
            var stream = new SqlFileStream(path, transactionContext, FileAccess.Read);
            yield return new DocumentLoadResponseFromStream(stream, id);
        }

        await transaction.CommitAsync();
    }

    public async Task<Guid> SaveAsync(StorageSaveRequest saveRequest, CancellationToken cancellationToken)
    {
        await using var sqlConnection = _dbConnectionFactory.CreateConnection();

        var command = await CheckFileExistsAsync(sqlConnection, saveRequest.Id, cancellationToken)
            ? FileSqlCommands.CreateUpdateFileCommand(sqlConnection, saveRequest.Id)
            : FileSqlCommands.CreateInsertFileCommand(sqlConnection, saveRequest.Id);

        await HandleFileWithCommand(sqlConnection, command, saveRequest, cancellationToken);
        
        return saveRequest.Id;
    }

    private static async Task HandleFileWithCommand(
        SqlConnection sqlConnection, SqlCommand command, StorageSaveRequest saveRequest, CancellationToken cancellationToken)
    {
        var transactionContext = Array.Empty<byte>();
        var path = string.Empty;

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
        await command.DisposeAsync();
    }
    
    private static async Task<bool> CheckFileExistsAsync(
        SqlConnection sqlConnection, Guid id, CancellationToken cancellationToken)
    {
        await using var command = FileSqlCommands.CreateCheckFileCommand(sqlConnection, id);
        var count = await command.ExecuteScalarAsync(cancellationToken);
        return Convert.ToInt32(count) != 0;
    }
    
    private static byte[] ReadTransactionContext(SqlDataReader reader) => (byte[])reader["transactionContext"];
    private static string ReadFilePath(SqlDataReader reader) => reader["filePath"].ToString()!;
    private static Guid ReadFileId(SqlDataReader reader) => Guid.Parse(reader["id"].ToString()!);
}