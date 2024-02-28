﻿using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlTypes;
using PhlegmaticOne.UniDocuments.Data.Files.Base;
using PhlegmaticOne.UniDocuments.Data.Files.Models;

namespace PhlegmaticOne.UniDocuments.Data.Files.Sql;

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

        using var command = FileSqlCommands.CreateSelectFileCommand(_sqlConnection, fileId);
        using var transaction = _sqlConnection.BeginTransaction();
        command.Transaction = transaction;

        using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                transactionContext = ReadTransactionContext(reader);
                path = ReadFilePath(reader);
                savedFileName = ReadFileName(reader);
            }
        }

        var destinationStream = new MemoryStream();
        using (var source = new SqlFileStream(path, transactionContext, FileAccess.Read))
        {
            source.CopyTo(destinationStream);
        }

        await command.Transaction.CommitAsync();

        return new FileLoadResponse(fileId, savedFileName, destinationStream);
    }

    public async Task SaveFileAsync(FileLocalSaveRequest fileLocalSaveRequest)
    {
        var fileName = fileLocalSaveRequest.FileName;
        var transactionContext = Array.Empty<byte>();
        var path = string.Empty;

        using var command = FileSqlCommands.CreateInsertFileCommand(_sqlConnection, fileName);
        using var transaction = _sqlConnection.BeginTransaction();
        command.Transaction = transaction;

        using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                transactionContext = ReadTransactionContext(reader);
                path = ReadFilePath(reader);
            }
        }

        using (var destination = new SqlFileStream(path, transactionContext, FileAccess.Write))
        {
            using var source = File.OpenRead(fileLocalSaveRequest.LocalFilePath);
            await source.CopyToAsync(destination);
        }

        await command.Transaction.CommitAsync();
    }

    private static byte[] ReadTransactionContext(SqlDataReader reader) => (byte[])reader["transactionContext"];
    private static string ReadFilePath(SqlDataReader reader) => reader["filePath"].ToString()!;
    private static string ReadFileName(SqlDataReader reader) => reader["FileName"].ToString()!;
}