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
                    SELECT @fileId, 0x, @fileName";

    private const string SelectFileCommandText = @"
                    SELECT
                        Content.PathName() AS filePath,
                        GET_FILESTREAM_TRANSACTION_CONTEXT() as transactionContext,
                        FileName as fileName
                    FROM [uni_documents_db].[dbo].[StudyDocumentsContent]
                    WHERE Id = @fileId";

    private const string SelectFilesCommandText = @"
                    SELECT
                        [Content].PathName() AS filePath,
                        GET_FILESTREAM_TRANSACTION_CONTEXT() as transactionContext,
                        FileName as fileName,
                        Id as id
                    FROM [uni_documents_db].[dbo].[StudyDocumentsContent]
                    WHERE Id IN ({0})";

    internal static SqlCommand CreateInsertFileCommand(SqlConnection sqlConnection, string fileName, Guid id)
    {
        return new SqlCommand(InsertFileCommandText, sqlConnection)
        {
            Parameters =
            {
                new SqlParameter
                {
                    ParameterName = "@fileName",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = fileName
                },
                new SqlParameter
                {
                    ParameterName = "@fileId",
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Value = id
                }
            }
        };
    }

    internal static SqlCommand CreateSelectFileCommand(SqlConnection sqlConnection, Guid fileId)
    {
        return new SqlCommand(SelectFileCommandText, sqlConnection)
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
}