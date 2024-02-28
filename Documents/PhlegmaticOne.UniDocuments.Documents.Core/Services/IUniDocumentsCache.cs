namespace PhlegmaticOne.UniDocuments.Documents.Core.Services;

public interface IUniDocumentsCache
{
    Task CacheDocumentAsync(UniDocument document);
    Task<UniDocument> GetDocumentAsync(Guid documentId);
}
