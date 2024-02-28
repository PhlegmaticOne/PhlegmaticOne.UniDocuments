using Microsoft.Data.SqlClient;

namespace PhlegmaticOne.UniDocuments.Data.Files.Sql;

internal class FileSqlCommands
{
    private const string InsertFileCommandText = @"
                    INSERT INTO [uni_documents_db].[dbo].[StudyDocumentsContent]
                    OUTPUT 
                        GET_FILESTREAM_TRANSACTION_CONTEXT() AS transactionContext, 
                        inserted.Content.PathName() AS filePath
                    SELECT NEWID(), 0x, @fileName";

    private const string SelectFileCommandText = @"
                    SELECT
                        Content.PathName() AS filePath,
                        GET_FILESTREAM_TRANSACTION_CONTEXT() as transactionContext,
                        FileName as fileName
                    FROM [uni_documents_db].[dbo].[StudyDocumentsContent]
                    WHERE Id = @fileId";

    internal static SqlCommand CreateInsertFileCommand(
        SqlConnection sqlConnection, string fileName)
    {
        return new SqlCommand(InsertFileCommandText, sqlConnection)
        {
            Parameters =
            {
                new SqlParameter
                {
                    ParameterName = "@fileName",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Value = fileName
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
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                    Value = fileId
                }
            }
        };
    }
}
