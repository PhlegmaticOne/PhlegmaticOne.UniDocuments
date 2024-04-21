using UniDocuments.Text.Domain.Services.DocumentMapping.Models;

namespace UniDocuments.Text.Domain.Services.DocumentMapping;

public interface IDocumentMapper
{
    int GetDocumentIdFromGlobalParagraphId(int paragraphId);
    DocumentGlobalMapData? GetDocumentData(int documentId);
    int GetDocumentId(Guid documentId);
    void AddDocument(Guid id, int paragraphsCount, string name);
}