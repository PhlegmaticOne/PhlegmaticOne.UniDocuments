using Microsoft.Data.SqlClient;
using UniDocuments.Text.Domain.Services.DocumentNameMapping;
using UniDocuments.Text.Domain.Services.FileStorage;

namespace UniDocuments.Text.Services.DocumentNameMapping;

public class DocumentToNameMapperSql : IDocumentToNameMapper
{
    private const string SelectFilesCommandText = @"
                    SELECT
                        Id as id,
                        FileName as fileName
                    FROM [uni_documents_db].[dbo].[StudyDocumentsContent]";
    
    private readonly ISqlConnectionProvider _sqlConnectionProvider;
    private readonly Dictionary<Guid, string> _documentNamesMap = new();

    public DocumentToNameMapperSql(ISqlConnectionProvider sqlConnectionProvider)
    {
        _sqlConnectionProvider = sqlConnectionProvider;
    }
    
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await using var command = CreateSelectAllFilesCommand(_sqlConnectionProvider.Connection);
        var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            var id = ReadFileId(reader);
            var name = ReadFileName(reader);
            _documentNamesMap.Add(id, name);
        }
    }

    public string GetDocumentName(Guid documentId)
    {
        return _documentNamesMap[documentId];
    }

    public void AddMap(Guid documentId, string documentName)
    {
        _documentNamesMap.TryAdd(documentId, documentName);
    }
    
    private static string ReadFileName(SqlDataReader reader) => reader["fileName"].ToString()!;
    private static Guid ReadFileId(SqlDataReader reader) => Guid.Parse(reader["id"].ToString()!);

    private static SqlCommand CreateSelectAllFilesCommand(SqlConnection sqlConnection)
    {
        return new SqlCommand(SelectFilesCommandText, sqlConnection);
    }
}