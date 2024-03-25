using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Services.Documents;

namespace UniDocuments.Text.Services.Documents;

public class UniDocumentsService : IUniDocumentsService
{
    private readonly IUniDocumentsCache _cache;

    public UniDocumentsService(IUniDocumentsCache cache)
    {
        _cache = cache;
    }
    
    public Task<UniDocument> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var document = _cache.Get(id) ?? new UniDocument(id);
        return Task.FromResult(document);
    }

    public Task SaveAsync(UniDocument document, CancellationToken cancellationToken)
    {
        _cache.Cache(document);
        return Task.CompletedTask;
    }
}