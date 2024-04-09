using Microsoft.Data.SqlClient;
using UniDocuments.App.Services.FileStorage.Sql.Connection;
using UniDocuments.Text.Domain.Services.Common;

namespace UniDocuments.Text.Application.Mapper;

public class DocumentsMapperSql : IDocumentsMapper
{
    private const string SelectFilesCommandText = @"
                    SELECT
                        Id as id,
                        FileName as fileName
                    FROM [uni_documents_db].[dbo].[StudyDocumentsContent]";
    
    private readonly ISqlConnectionProvider _sqlConnectionProvider;
    private readonly Dictionary<Guid, string> _documentNamesMap = new();

    public DocumentsMapperSql(ISqlConnectionProvider sqlConnectionProvider)
    {
        _sqlConnectionProvider = sqlConnectionProvider;
    }
    
    public async Task InitializeAsync()
    {
        await using var command = CreateSelectAllFilesCommand(_sqlConnectionProvider.Connection);
        var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
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