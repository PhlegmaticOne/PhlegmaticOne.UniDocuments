using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlTypes;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.Text.Services.Neural.Services;

public class DocumentNeuralSourceSql : IDocumentsNeuralSource
{
    private const string SelectFilesCommandText = @"
                    SELECT
                        Id as id,
                        Content.PathName() AS filePath,
                        GET_FILESTREAM_TRANSACTION_CONTEXT() as transactionContext
                    FROM [uni_documents_db].[dbo].[StudyDocumentsContent]";
    
    private readonly ISqlConnectionProvider _sqlConnectionProvider;
    private readonly IStreamContentReader _streamContentReader;

    private SqlDataReader? _dataReader;

    public DocumentNeuralSourceSql(ISqlConnectionProvider sqlConnectionProvider, IStreamContentReader streamContentReader)
    {
        _sqlConnectionProvider = sqlConnectionProvider;
        _streamContentReader = streamContentReader;
    }
    
    public async Task InitializeAsync()
    {
        var command = CreateSelectAllFilesCommand(_sqlConnectionProvider.Connection);
        _dataReader = await command.ExecuteReaderAsync();
    }

    public async Task<UniDocument> GetNextDocumentAsync()
    {
        var hasData = await _dataReader!.ReadAsync();

        if (!hasData)
        {
            return UniDocument.Empty;
        }

        var transactionContext = ReadTransactionContext(_dataReader);
        var path = ReadFilePath(_dataReader);
        var id = ReadFileId(_dataReader);
        
        await using var source = new SqlFileStream(path, transactionContext, FileAccess.Read);
        var content = await _streamContentReader.ReadAsync(source, CancellationToken.None);
        var document = new UniDocument(id, content);
        return document;
    }

    public void Dispose()
    {
        _dataReader!.Dispose();
        _dataReader = null;
    }
    
    private static byte[] ReadTransactionContext(SqlDataReader reader) => (byte[])reader["transactionContext"];
    private static string ReadFilePath(SqlDataReader reader) => reader["filePath"].ToString()!;
    private static Guid ReadFileId(SqlDataReader reader) => Guid.Parse(reader["id"].ToString()!);

    private static SqlCommand CreateSelectAllFilesCommand(SqlConnection sqlConnection)
    {
        return new SqlCommand(SelectFilesCommandText, sqlConnection);
    }
}