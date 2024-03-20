namespace UniDocuments.Text.Domain.Services.Documents;

public interface IUniDocumentsCache
{
    Task CacheDocumentAsync(UniDocument document);
    Task<UniDocument?> GetDocumentAsync(Guid documentId);
}
