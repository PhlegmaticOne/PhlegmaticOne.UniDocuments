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

    public async Task<DocumentLoadResponse> LoadAsync(Guid id, CancellationToken cancellationToken)
    {
        var transactionContext = Array.Empty<byte>();
        var savedFileName = string.Empty;
        var path = string.Empty;
        var sqlConnection = _sqlConnectionProvider.Connection;

        await using var command = FileSqlCommands.CreateSelectFileCommand(sqlConnection, id);
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

        try
        {
            var stream = new SqlFileStream(path, transactionContext, FileAccess.Read);
            return new DocumentLoadResponse(savedFileName, stream);
            // await using (var source = new SqlFileStream(path, transactionContext, FileAccess.Read))
            // {
            //     var destinationStream = new MemoryStream();
            //
            //     await source.CopyToAsync(destinationStream, cancellationToken);
            //
            //     destinationStream.Position = 0;
            //     
            //     return new DocumentLoadResponse(savedFileName, destinationStream);
            // }
        }
        catch(Exception e)
        {
            return DocumentLoadResponse.NoContent();
        }
    }

    public async Task<Guid> SaveAsync(DocumentSaveRequest saveRequest, CancellationToken cancellationToken)
    {
        var fileName = saveRequest.Name;
        var transactionContext = Array.Empty<byte>();
        var path = string.Empty;
        var sqlConnection = _sqlConnectionProvider.Connection;

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
}