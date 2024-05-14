using KeyedSemaphores;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.ContentReading;
using UniDocuments.Text.Domain.Services.Cache;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.Text.Application.ContentReading;

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
            var stream = loadResponse.ToStream();
            var content = await _streamContentReader.ReadAsync(stream, cancellationToken);
            var result = new UniDocument(documentId, content);

            if (cache)
            {
                _documentsCache.Cache(result);
            }

            await stream.DisposeAsync();
            return result;
        }
    }

    public async Task<Dictionary<Guid, UniDocument>> LoadAsync(ISet<Guid> documentIds, bool cache, CancellationToken cancellationToken)
    {
        var result = new Dictionary<Guid, UniDocument>();

        foreach (var documentId in documentIds)
        {
            var document = await LoadAsync(documentId, cache, cancellationToken);
            result.Add(documentId, document);
        }

        return result;
    }
}