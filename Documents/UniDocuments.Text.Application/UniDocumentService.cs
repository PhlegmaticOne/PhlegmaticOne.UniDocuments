using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Services.Documents;

namespace UniDocuments.Text.Application;

public class UniDocumentService : IUniDocumentsService
{
    private readonly IUniDocumentsCache _cache;

    public UniDocumentService(IUniDocumentsCache cache)
    {
        _cache = cache;
    }

    public async Task<UniDocument> GetDocumentAsync(Guid id)
    {
        var cachedDocument = await _cache.GetDocumentAsync(id) ?? new UniDocument(id);
        await _cache.CacheDocumentAsync(cachedDocument);
        return cachedDocument;
    }
}
