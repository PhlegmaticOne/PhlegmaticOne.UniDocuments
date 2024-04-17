using UniDocuments.Text.Domain.Services.DocumentMapping;

namespace UniDocuments.Text.Services.DocumentMapping;

public class DocumentMapperInMemory : IDocumentMapper
{
    private readonly Dictionary<Guid, string> _documentsMap = new();
    private readonly Dictionary<int, Guid> _numberToIdsMap = new();
    
    public Task InitializeAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public string GetDocumentName(Guid documentId)
    {
        return _documentsMap[documentId];
    }

    public Guid GetDocumentId(int id)
    {
        return _numberToIdsMap[id];
    }

    public void AddMap(Guid documentId, string documentName)
    {
        if (_documentsMap.TryAdd(documentId, documentName))
        {
            _numberToIdsMap[_documentsMap.Count - 1] = documentId;
        }
    }
}