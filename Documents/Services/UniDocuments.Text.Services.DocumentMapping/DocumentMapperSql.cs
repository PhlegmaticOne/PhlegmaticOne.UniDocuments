using Microsoft.Data.SqlClient;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.FileStorage;

namespace UniDocuments.Text.Services.DocumentMapping;

public class DocumentMapperSql : IDocumentMapper
{
    private const string SelectFilesCommandText = @"
                    SELECT
                        Id as id,
                        FileName as fileName
                    FROM [uni_documents_db].[dbo].[StudyDocumentsContent]";
    
    private readonly ISqlConnectionProvider _sqlConnectionProvider;
    private readonly Dictionary<Guid, string> _documentNamesMap = new();
    private readonly Dictionary<int, Guid> _numberToIdsMap = new();

    public DocumentMapperSql(ISqlConnectionProvider sqlConnectionProvider)
    {
        _sqlConnectionProvider = sqlConnectionProvider;
    }
    
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await using var command = CreateSelectAllFilesCommand(_sqlConnectionProvider.Connection);
        var reader = await command.ExecuteReaderAsync(cancellationToken);
        var i = 0;

        while (await reader.ReadAsync(cancellationToken))
        {
            var id = ReadFileId(reader);
            var name = ReadFileName(reader);
            _documentNamesMap.Add(id, name);
            _numberToIdsMap.Add(i, id);
            i++;
        }
    }

    public string GetDocumentName(Guid documentId)
    {
        return _documentNamesMap[documentId];
    }

    public Guid GetDocumentId(int id)
    {
        return _numberToIdsMap[id];
    }

    public void AddMap(Guid documentId, string documentName)
    {
        if (_documentNamesMap.TryAdd(documentId, documentName))
        {
            _numberToIdsMap[_documentNamesMap.Count - 1] = documentId;
        }
    }
    
    private static string ReadFileName(SqlDataReader reader) => reader["fileName"].ToString()!;
    private static Guid ReadFileId(SqlDataReader reader) => Guid.Parse(reader["id"].ToString()!);

    private static SqlCommand CreateSelectAllFilesCommand(SqlConnection sqlConnection)
    {
        return new SqlCommand(SelectFilesCommandText, sqlConnection);
    }
}