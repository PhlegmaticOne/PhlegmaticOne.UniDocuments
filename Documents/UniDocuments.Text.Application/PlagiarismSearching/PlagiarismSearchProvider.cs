using UniDocuments.Text.Domain.Providers.Loading;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;
using UniDocuments.Text.Domain.Services.Neural;

namespace UniDocuments.Text.Application.PlagiarismSearching;

public class PlagiarismSearchProvider : IPlagiarismSearchProvider
{
    private readonly IDocumentsNeuralModel _documentsNeuralModel;
    private readonly IFingerprintSearcher _fingerprintSearcher;
    private readonly IDocumentLoadingProvider _documentLoadingProvider;

    public PlagiarismSearchProvider(
        IDocumentsNeuralModel documentsNeuralModel, 
        IFingerprintSearcher fingerprintSearcher,
        IDocumentLoadingProvider documentLoadingProvider)
    {
        _documentsNeuralModel = documentsNeuralModel;
        _fingerprintSearcher = fingerprintSearcher;
        _documentLoadingProvider = documentLoadingProvider;
    }
    
    public async Task<PlagiarismSearchResponse> SearchAsync(
        PlagiarismSearchRequest request, CancellationToken cancellationToken)
    {
        var documentId = request.DocumentId;
        var document = await _documentLoadingProvider.LoadAsync(documentId, false, cancellationToken);
        
        var topFingerprints = await _fingerprintSearcher
            .SearchTopAsync(documentId, request.NDocuments, cancellationToken);
        
        var topParagraphs = await _documentsNeuralModel
            .FindSimilarAsync(document, request.NDocuments, cancellationToken);
        
        return new PlagiarismSearchResponse(topParagraphs, topFingerprints);
    }
}