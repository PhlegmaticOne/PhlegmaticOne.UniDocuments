using KeyedSemaphores;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.ContentReading;
using UniDocuments.Text.Domain.Services.Cache;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;
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
    
    public async Task<UniDocument?> LoadAsync(Guid documentId, bool cache, CancellationToken cancellationToken)
    {
        using (await KeyedSemaphore.LockAsync(documentId.ToString(), cancellationToken))
        {
            var cached = _documentsCache.Get(documentId);

            if (cached is not null)
            {
                return cached;
            }

            var loadResponse = await _documentsStorage.LoadAsync(documentId, cancellationToken);
            return loadResponse is null ? null : CreateDocument(loadResponse, cache);
        }
    }

    public async Task<Dictionary<Guid, UniDocument>> LoadAsync(ISet<Guid> documentIds, bool cache, CancellationToken cancellationToken)
    {
        var finding = new List<Guid>(documentIds);
        var result = new Dictionary<Guid, UniDocument>();

        for (var i = finding.Count - 1; i >= 0; i--)
        {
            var saved = _documentsCache.Get(finding[i]);

            if (saved is not null)
            {
                _documentsCache.Cache(saved);
                result.Add(finding[i], saved);
                finding.RemoveAt(i);
            }
        }

        if (finding.Count == 0)
        {
            return result;
        }
        
        await foreach (var response in _documentsStorage.LoadAsync(finding, cancellationToken).ConfigureAwait(false))
        {
            result.Add(response.Id, CreateDocument(response, cache));
        }

        return result;
    }

    private UniDocument CreateDocument(IDocumentLoadResponse loadResponse, bool cache)
    {
        var stream = loadResponse.ToStream();
        var content = _streamContentReader.Read(stream);
        var result = new UniDocument(loadResponse.Id, content);

        if (cache)
        {
            _documentsCache.Cache(result);
        }

        stream.Dispose();
        return result;
    }
}