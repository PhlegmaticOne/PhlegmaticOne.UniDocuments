using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.Text.Domain.Services.DocumentMapping;

namespace UniDocuments.Text.Services.DocumentMapping;

public class DocumentMapper : IDocumentMapper
{
    private readonly Dictionary<Guid, string> _documentNamesMap = new();
    private readonly Dictionary<int, Guid> _numberToIdsMap = new();

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
}