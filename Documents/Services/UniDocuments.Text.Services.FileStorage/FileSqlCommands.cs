using System.Data;
using Microsoft.Data.SqlClient;

namespace UniDocuments.Text.Services.FileStorage;

internal static class FileSqlCommands
{
    private const string InsertFileCommandText = @"
                    INSERT INTO [uni_documents_db].[dbo].[StudyDocumentsContent]
                    OUTPUT 
                        GET_FILESTREAM_TRANSACTION_CONTEXT() AS transactionContext, 
                        inserted.Content.PathName() AS filePath
                    SELECT @fileId, 0x";

    private const string SelectFileCommandText = @"
                    SELECT Top(1)
                        Content.PathName() AS filePath,
                        GET_FILESTREAM_TRANSACTION_CONTEXT() as transactionContext
                    FROM [uni_documents_db].[dbo].[StudyDocumentsContent]
                    WHERE Id = @fileId";

    private const string SelectFilesCommandText = @"
                    SELECT
                        [Content].PathName() AS filePath,
                        GET_FILESTREAM_TRANSACTION_CONTEXT() as transactionContext,
                        Id as id
                    FROM [uni_documents_db].[dbo].[StudyDocumentsContent]
                    WHERE Id IN ({0})";

    private const string UpdateFileCommandText = @"
                    SELECT TOP(1)
                        Content.PathName() AS filePath,
                        GET_FILESTREAM_TRANSACTION_CONTEXT() as transactionContext
                    FROM [uni_documents_db].[dbo].[StudyDocumentsContent]
                    WHERE Id = @fileId";

    private const string CheckFileExistsCommandText = @"
                    SELECT COUNT(*) FROM [uni_documents_db].[dbo].[StudyDocumentsContent]
                    WHERE Id = @fileId";

    internal static SqlCommand CreateCheckFileCommand(SqlConnection sqlConnection, Guid id)
    {
        return CreateCommandWithFileId(sqlConnection, CheckFileExistsCommandText, id);
    }
    
    internal static SqlCommand CreateUpdateFileCommand(SqlConnection sqlConnection, Guid id)
    {
        return CreateCommandWithFileId(sqlConnection, UpdateFileCommandText, id);
    }

    internal static SqlCommand CreateSelectFileCommand(SqlConnection sqlConnection, Guid id)
    {
        return CreateCommandWithFileId(sqlConnection, SelectFileCommandText, id);
    }

    internal static SqlCommand CreateInsertFileCommand(SqlConnection sqlConnection, Guid id)
    {
        return CreateCommandWithFileId(sqlConnection, InsertFileCommandText, id);
    }

    internal static SqlCommand CreateSelectFilesCommand(SqlConnection sqlConnection, IList<Guid> ids)
    {
        var parameters = new string[ids.Count];
        var command = new SqlCommand();
        
        for (var i = 0; i < ids.Count; i++)
        {
            parameters[i] = $"@Id{i}";
            command.Parameters.AddWithValue(parameters[i], ids[i]);
        }

        command.CommandText = string.Format(SelectFilesCommandText, string.Join(", ", parameters));
        command.Connection = sqlConnection;

        return command;
    }
    
    private static SqlCommand CreateCommandWithFileId(SqlConnection sqlConnection, string commandText, Guid fileId)
    {
        return new SqlCommand(commandText, sqlConnection)
        {
            Parameters =
            {
                new SqlParameter
                {
                    ParameterName = "@fileId",
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Value = fileId
                }
            }
        };
    } 
}