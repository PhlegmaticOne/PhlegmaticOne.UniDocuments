using UniDocuments.Text.Domain.Services.DocumentNameMapping;

namespace UniDocuments.Text.Services.DocumentNameMapping;

public class DocumentToNameMapperInMemory : IDocumentToNameMapper
{
    private readonly Dictionary<Guid, string> _documentsMap = new();
    
    public Task InitializeAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public string GetDocumentName(Guid documentId)
    {
        return _documentsMap[documentId];
    }

    public void AddMap(Guid documentId, string documentName)
    {
        _documentsMap.TryAdd(documentId, documentName);
    }
}