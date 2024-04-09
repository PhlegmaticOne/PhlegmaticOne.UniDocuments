using UniDocuments.Text.Domain.Services.Common;

namespace UniDocuments.Text.Application.Mapper;

public class DocumentsMapperInMemory : IDocumentsMapper
{
    private readonly Dictionary<Guid, string> _documentsMap = new();
    
    public Task InitializeAsync()
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