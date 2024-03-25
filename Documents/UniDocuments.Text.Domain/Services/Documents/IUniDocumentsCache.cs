namespace UniDocuments.Text.Domain.Services.Documents;

public interface IUniDocumentsCache
{
    void Cache(UniDocument document);
    UniDocument? Get(Guid documentId);
}
