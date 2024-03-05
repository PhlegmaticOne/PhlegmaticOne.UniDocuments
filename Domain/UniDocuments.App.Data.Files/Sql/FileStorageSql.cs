using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlTypes;
using UniDocuments.App.Data.Files.Base;
using UniDocuments.App.Data.Files.Models;

namespace UniDocuments.App.Data.Files.Sql;

public class FileStorageSql : IFileStorage
{
    private readonly SqlConnection _sqlConnection;

    public FileStorageSql(string connectionString)
    {
        _sqlConnection = new SqlConnection(connectionString);
        _sqlConnection.Open();
    }

    public Task<IList<FileLoadResponse>> GetFilesPagedAsync(int pageIndex, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<FileLoadResponse> LoadFileAsync(Guid fileId)
    {
        var transactionContext = Array.Empty<byte>();
        var savedFileName = string.Empty;
        var path = string.Empty;

        await using var command = FileSqlCommands.CreateSelectFileCommand(_sqlConnection, fileId);
        await using var transaction = _sqlConnection.BeginTransaction();
        command.Transaction = transaction;

        await using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                transactionContext = ReadTransactionContext(reader);
                path = ReadFilePath(reader);
                savedFileName = ReadFileName(reader);
            }
        }

        var destinationStream = new MemoryStream();
        
        await using (var source = new SqlFileStream(path, transactionContext, FileAccess.Read))
        {
            await source.CopyToAsync(destinationStream);
        }

        await command.Transaction.CommitAsync();

        return new FileLoadResponse(fileId, savedFileName, destinationStream);
    }

    public async Task SaveFileAsync(FileLocalSaveRequest fileLocalSaveRequest)
    {
        var fileName = fileLocalSaveRequest.FileName;
        var transactionContext = Array.Empty<byte>();
        var path = string.Empty;

        await using var command = FileSqlCommands.CreateInsertFileCommand(_sqlConnection, fileName);
        await using var transaction = _sqlConnection.BeginTransaction();
        command.Transaction = transaction;

        await using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                transactionContext = ReadTransactionContext(reader);
                path = ReadFilePath(reader);
            }
        }

        await using (var destination = new SqlFileStream(path, transactionContext, FileAccess.Write))
        {
            await using var source = File.OpenRead(fileLocalSaveRequest.LocalFilePath);
            await source.CopyToAsync(destination);
        }

        await command.Transaction.CommitAsync();
    }

    private static byte[] ReadTransactionContext(SqlDataReader reader) => (byte[])reader["transactionContext"];
    private static string ReadFilePath(SqlDataReader reader) => reader["filePath"].ToString()!;
    private static string ReadFileName(SqlDataReader reader) => reader["FileName"].ToString()!;
}