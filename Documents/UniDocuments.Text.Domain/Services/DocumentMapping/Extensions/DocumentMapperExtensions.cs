using UniDocuments.Text.Domain.Services.DocumentMapping.Models;

namespace UniDocuments.Text.Domain.Services.DocumentMapping.Extensions;

public static class DocumentMapperExtensions
{
    public static void AddDocument(this IDocumentMapper documentMapper, UniDocument document, string name)
    {
        documentMapper.AddDocument(document.Id, document.Content!.ParagraphsCount, name);
    }
    
    public static DocumentGlobalMapData? GetDocumentData(this IDocumentMapper documentMapper, Guid documentId)
    {
        var id = documentMapper.GetDocumentId(documentId);
        return documentMapper.GetDocumentData(id);
    }
}