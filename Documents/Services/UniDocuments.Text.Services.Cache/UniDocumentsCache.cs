using FastCache;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Services.Cache;

namespace UniDocuments.Text.Services.Cache;

public class UniDocumentsCache : IUniDocumentsCache
{
    private static readonly TimeSpan CacheTime = TimeSpan.FromMinutes(10);
    
    public void Cache(UniDocument document)
    {
        Cached<UniDocument>.Save(document.Id, document, CacheTime);
    }

    public UniDocument? Get(Guid documentId)
    {
        return Cached<UniDocument>.TryGet(documentId, out var cached) ? cached : null;
    }
}