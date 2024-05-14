using UniDocuments.Text.Domain.Services.DocumentMapping.Models;

namespace UniDocuments.Text.Domain.Services.DocumentMapping.Extensions;

public static class DocumentMapperExtensions
{
    public static void AddDocument(this IDocumentMapper documentMapper, UniDocument document)
    {
        documentMapper.AddDocument(document.Id, document.Content!.ParagraphsCount);
    }
    
    public static DocumentGlobalMapData? GetDocumentData(this IDocumentMapper documentMapper, Guid id)
    {
        var idInt = documentMapper.GetDocumentId(id);
        return documentMapper.GetDocumentData(idInt);
    }
}