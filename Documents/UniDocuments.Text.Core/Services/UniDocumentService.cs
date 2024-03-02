using UniDocuments.Text.Core.Features;

namespace UniDocuments.Text.Core.Services;

public class UniDocumentService : IUniDocumentsService
{
    private readonly Dictionary<UniDocumentFeatureFlag, IUniDocumentFeatureFactory> _factories;
    private readonly IUniDocumentsCache _cache;

    public UniDocumentService(IUniDocumentsCache cache, IEnumerable<IUniDocumentFeatureFactory> featureFactories)
    {
        _cache = cache;
        _factories = featureFactories.ToDictionary(x => x.FeatureFlag, x => x);
    }

    public async Task<UniDocument> GetDocumentAsync(Guid id, params UniDocumentFeatureFlag[] featureFlags)
    {
        var cachedDocument = await _cache.GetDocumentAsync(id);

        if (cachedDocument is null)
        {
            cachedDocument = await CreateNewDocument(id, featureFlags);
        }
        else
        {
            await EnsureAllFeaturesExists(cachedDocument, featureFlags);
        }

        await _cache.CacheDocumentAsync(cachedDocument);

        return cachedDocument;
    }

    private async Task<UniDocument> CreateNewDocument(Guid id, params UniDocumentFeatureFlag[] featureFlags)
    {
        var document = new UniDocument(id);

        foreach (var featureFlag in featureFlags)
        {
            var feature = await CreateFeature(featureFlag, id);
            document.AddFeature(feature);
        }

        return document;
    }

    private async Task EnsureAllFeaturesExists(UniDocument document, params UniDocumentFeatureFlag[] featureFlags)
    {
        foreach (var featureFlag in featureFlags)
        {
            if (document.ContainsFeature(featureFlag) == false)
            {
                var feature = await CreateFeature(featureFlag, document.Id);
                document.AddFeature(feature);
            }
        }
    }
    
    private Task<IUniDocumentFeature> CreateFeature(UniDocumentFeatureFlag featureFlag, Guid id)
    {
        var featureFactory = _factories[featureFlag];
        return featureFactory.CreateFeature(id);
    }
}
