using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlTypes;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.DocumentMapping.Models;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.Text.Services.Neural.Sources;

public class DocumentNeuralSourceSql : IDocumentsNeuralSource
{
    private const string SelectFilesCommandText = @"
                    SELECT
                        Id as id,
                        Content.PathName() AS filePath,
                        GET_FILESTREAM_TRANSACTION_CONTEXT() as transactionContext
                    FROM [uni_documents_db].[dbo].[StudyDocumentsContent]";
    
    private readonly ISqlConnectionProvider _sqlConnectionProvider;
    private readonly IDocumentMapper _documentMapper;
    private readonly IStreamContentReader _streamContentReader;

    private SqlDataReader? _dataReader;

    public DocumentNeuralSourceSql(
        ISqlConnectionProvider sqlConnectionProvider,
        IDocumentMapper documentMapper,
        IStreamContentReader streamContentReader)
    {
        _sqlConnectionProvider = sqlConnectionProvider;
        _documentMapper = documentMapper;
        _streamContentReader = streamContentReader;
    }
    
    public async Task InitializeAsync()
    {
        var command = CreateSelectAllFilesCommand(_sqlConnectionProvider.Connection);
        _dataReader = await command.ExecuteReaderAsync();
    }

    public async Task<DocumentNeuralViewModel> GetNextDocumentAsync()
    {
        var hasData = await _dataReader!.ReadAsync();

        if (!hasData)
        {
            return DocumentNeuralViewModel.Empty;
        }

        var transactionContext = ReadTransactionContext(_dataReader);
        var path = ReadFilePath(_dataReader);
        var id = ReadFileId(_dataReader);
        
        await using var source = new SqlFileStream(path, transactionContext, FileAccess.Read);
        var content = await _streamContentReader.ReadAsync(source, CancellationToken.None);
        var documentData = _documentMapper.GetDocumentData(id);
        var resultParagraphs = CreateParagraphs(content, documentData);
        
        return new DocumentNeuralViewModel(0, resultParagraphs);
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
    
    private static List<ParagraphNeuralViewModel> CreateParagraphs(
        UniDocumentContent content, DocumentGlobalMapData documentData)
    {
        var resultParagraphs = new List<ParagraphNeuralViewModel>();

        for (var i = 0; i < content.Paragraphs.Count; i++)
        {
            var paragraph = content.Paragraphs[i];
            var globalId = documentData.GlobalFirstParagraphId + i;
            resultParagraphs.Add(new ParagraphNeuralViewModel(globalId, paragraph));
        }

        return resultParagraphs;
    }
}