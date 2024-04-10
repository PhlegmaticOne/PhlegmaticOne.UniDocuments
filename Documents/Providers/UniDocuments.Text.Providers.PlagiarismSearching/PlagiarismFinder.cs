using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Requests;
using UniDocuments.Text.Features.Fingerprint.Services;
using UniDocuments.Text.Features.Text.Contracts;

namespace UniDocuments.Text.Providers.PlagiarismSearching;

public class PlagiarismFinder : IPlagiarismFinder
{
    private readonly IDocumentsNeuralModel _documentsNeuralModel;
    private readonly IFingerprintSearcher _fingerprintSearcher;
    private readonly IDocumentTextLoader _textLoader;

    public PlagiarismFinder(
        IDocumentsNeuralModel documentsNeuralModel, 
        IFingerprintSearcher fingerprintSearcher,
        IDocumentTextLoader textLoader)
    {
        _documentsNeuralModel = documentsNeuralModel;
        _fingerprintSearcher = fingerprintSearcher;
        _textLoader = textLoader;
    }
    
    public async Task<PlagiarismSearchResponse> SearchAsync(
        PlagiarismSearchRequest request, CancellationToken cancellationToken)
    {
        var token = CancellationToken.None;
        var documentId = request.DocumentId;
        var content = await _textLoader.LoadTextAsync(request.DocumentId, token);
        
        var topFingerprintsTask = _fingerprintSearcher.SearchTopAsync(documentId, request.NDocuments, cancellationToken);
        var searchRequest = new NeuralSearchPlagiarismRequest(documentId, content, request.NDocuments);
        var topParagraphs = await _documentsNeuralModel.FindSimilarAsync(searchRequest, cancellationToken);
        var topFingerprints = await topFingerprintsTask;
        
        return new PlagiarismSearchResponse(topParagraphs, topFingerprints);
    }
}