using System.Data;
using Microsoft.Data.SqlClient;

namespace UniDocuments.Text.Services.FileStorage.Sql;

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

    internal static SqlCommand CreateInsertFileCommand(Microsoft.Data.SqlClient.SqlConnection sqlConnection, string fileName, Guid id)
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

    internal static SqlCommand CreateSelectFileCommand(Microsoft.Data.SqlClient.SqlConnection sqlConnection, Guid fileId)
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
}
