using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlTypes;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Requests;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;

namespace UniDocuments.Text.Services.FileStorage.Sql;

public class DocumentsStorageSql : IDocumentsStorage
{
    private readonly ISqlConnectionProvider _sqlConnectionProvider;

    public DocumentsStorageSql(ISqlConnectionProvider sqlConnectionProvider)
    {
        _sqlConnectionProvider = sqlConnectionProvider;
    }

    public async Task<DocumentLoadResponse> LoadAsync(DocumentLoadRequest loadRequest, CancellationToken cancellationToken)
    {
        var fileId = loadRequest.Id;
        var transactionContext = Array.Empty<byte>();
        var savedFileName = string.Empty;
        var path = string.Empty;
        var sqlConnection = _sqlConnectionProvider.Connection;

        await using var command = FileSqlCommands.CreateSelectFileCommand(sqlConnection, fileId);
        await using var transaction = sqlConnection.BeginTransaction();
        command.Transaction = transaction;

        await using (var reader = await command.ExecuteReaderAsync(cancellationToken))
        {
            while (await reader.ReadAsync(cancellationToken))
            {
                transactionContext = ReadTransactionContext(reader);
                path = ReadFilePath(reader);
                savedFileName = ReadFileName(reader);
            }
        }

        var destinationStream = new MemoryStream();
        
        await using (var source = new SqlFileStream(path, transactionContext, FileAccess.Read))
        {
            await source.CopyToAsync(destinationStream, cancellationToken);
        }

        await command.Transaction.CommitAsync(cancellationToken);

        return new DocumentLoadResponse(fileId, savedFileName, destinationStream);
    }

    public async Task<DocumentSaveResponse> SaveAsync(DocumentSaveRequest saveRequest, CancellationToken cancellationToken)
    {
        var fileName = saveRequest.Name;
        var transactionContext = Array.Empty<byte>();
        var path = string.Empty;
        var sqlConnection = _sqlConnectionProvider.Connection;

        await using var command = FileSqlCommands.CreateInsertFileCommand(sqlConnection, fileName);
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
        return new DocumentSaveResponse(Guid.Empty);
    }

    private static byte[] ReadTransactionContext(SqlDataReader reader) => (byte[])reader["transactionContext"];
    private static string ReadFilePath(SqlDataReader reader) => reader["filePath"].ToString()!;
    private static string ReadFileName(SqlDataReader reader) => reader["FileName"].ToString()!;
}