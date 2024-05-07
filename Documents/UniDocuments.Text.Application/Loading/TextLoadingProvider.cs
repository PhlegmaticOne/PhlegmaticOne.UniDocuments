using KeyedSemaphores;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.Loading;
using UniDocuments.Text.Domain.Services.Cache;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.Text.Application.Loading;

public class DocumentLoadingProvider : IDocumentLoadingProvider
{
    private readonly IDocumentsStorage _documentsStorage;
    private readonly IStreamContentReader _streamContentReader;
    private readonly IUniDocumentsCache _documentsCache;

    public DocumentLoadingProvider(
        IDocumentsStorage documentsStorage, 
        IStreamContentReader streamContentReader,
        IUniDocumentsCache documentsCache)
    {
        _documentsStorage = documentsStorage;
        _streamContentReader = streamContentReader;
        _documentsCache = documentsCache;
    }
    
    public async Task<UniDocument> LoadAsync(Guid documentId, bool cache, CancellationToken cancellationToken)
    {
        using (await KeyedSemaphore.LockAsync(documentId.ToString(), cancellationToken))
        {
            var cached = _documentsCache.Get(documentId);

            if (cached is not null)
            {
                return cached;
            }

            var loadResponse = await _documentsStorage.LoadAsync(documentId, cancellationToken);
            var content = await _streamContentReader.ReadAsync(loadResponse.Stream!, cancellationToken);
            var result = new UniDocument(documentId, content, loadResponse.Name);

            if (cache)
            {
                _documentsCache.Cache(result);
            }

            await loadResponse.DisposeAsync();
            return result;
        }
    }
}