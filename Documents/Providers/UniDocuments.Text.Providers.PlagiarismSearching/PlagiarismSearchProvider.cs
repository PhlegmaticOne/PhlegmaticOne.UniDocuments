using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Requests;

namespace UniDocuments.Text.Providers.PlagiarismSearching;

public class PlagiarismSearchProvider : IPlagiarismSearchProvider
{
    private readonly IDocumentsNeuralModel _documentsNeuralModel;
    private readonly IFingerprintSearcher _fingerprintSearcher;

    public PlagiarismSearchProvider(
        IDocumentsNeuralModel documentsNeuralModel, 
        IFingerprintSearcher fingerprintSearcher)
    {
        _documentsNeuralModel = documentsNeuralModel;
        _fingerprintSearcher = fingerprintSearcher;
    }
    
    public async Task<PlagiarismSearchResponse> SearchAsync(
        PlagiarismSearchRequest request, CancellationToken cancellationToken)
    {
        var documentId = request.DocumentId;
        
        var topFingerprints = await _fingerprintSearcher
            .SearchTopAsync(documentId, request.NDocuments, cancellationToken);
        
        var topParagraphs = await _documentsNeuralModel
            .FindSimilarAsync(new NeuralSearchPlagiarismRequest(documentId, request.NDocuments), cancellationToken);
        
        return new PlagiarismSearchResponse(topParagraphs, topFingerprints);
    }
}