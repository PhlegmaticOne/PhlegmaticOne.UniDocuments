namespace UniDocuments.Text.Domain.Services.Cache;

public interface IUniDocumentsCache
{
    void Cache(UniDocument document);
    UniDocument? Get(Guid documentId);
}
