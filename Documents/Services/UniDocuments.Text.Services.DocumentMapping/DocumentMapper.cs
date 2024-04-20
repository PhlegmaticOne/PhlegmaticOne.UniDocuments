using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.DocumentMapping.Models;

namespace UniDocuments.Text.Services.DocumentMapping;

public class DocumentMapper : IDocumentMapper
{
    private readonly List<int> _paragraphsToDocumentsMap;
    private readonly List<DocumentGlobalMapData> _documentsMap;
    private readonly Dictionary<Guid, int> _documentsIdMap;

    public DocumentMapper()
    {
        _paragraphsToDocumentsMap = new List<int>();
        _documentsMap = new List<DocumentGlobalMapData>();
        _documentsIdMap = new Dictionary<Guid, int>();
    }

    public int GetDocumentForGlobalParagraphId(int paragraphId)
    {
        return _paragraphsToDocumentsMap[paragraphId];
    }

    public DocumentGlobalMapData GetDocumentData(int documentId)
    {
        return _documentsMap[documentId];
    }

    public int GetDocumentId(Guid documentId)
    {
        return _documentsIdMap[documentId];
    }

    public void AddDocument(Guid id, int paragraphsCount, string name)
    {
        var globalLastDocumentId = _documentsMap.Count;
        var globalFirstParagraphId = _paragraphsToDocumentsMap.Count;
        
        MapNewParagraphs(paragraphsCount, globalLastDocumentId);
        MapNewDocument(id, name, globalFirstParagraphId);
        MapNewDocumentId(id, globalLastDocumentId);
    }

    private void MapNewParagraphs(int paragraphsCount, int globalLastDocumentId)
    {
        for (var i = 0; i < paragraphsCount; i++)
        {
            _paragraphsToDocumentsMap.Add(globalLastDocumentId);
        }
    }

    private void MapNewDocument(Guid id, string name, int globalFirstParagraphId)
    {
        _documentsMap.Add(new DocumentGlobalMapData(id, name, globalFirstParagraphId));
    }

    private void MapNewDocumentId(Guid id, int globalLastDocumentId)
    {
        _documentsIdMap[id] = globalLastDocumentId;
    }
}