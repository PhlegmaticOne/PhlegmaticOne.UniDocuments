namespace UniDocuments.Text.Domain.Services.DocumentMapping.Extensions;

public static class DocumentMapperExtensions
{
    public static void AddDocument(this IDocumentMapper documentMapper, UniDocument document)
    {
        documentMapper.AddDocument(document.Id, document.Content!.ParagraphsCount);
    }
}