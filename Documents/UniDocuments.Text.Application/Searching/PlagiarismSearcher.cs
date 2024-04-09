using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Searching;
using UniDocuments.Text.Domain.Services.Searching.Request;
using UniDocuments.Text.Domain.Services.Searching.Response;
using UniDocuments.Text.Features.Fingerprint.Services;
using UniDocuments.Text.Features.Text.Contracts;

namespace UniDocuments.Text.Application.Searching;

public class PlagiarismSearcher : IPlagiarismSearcher
{
    private readonly IDocumentTextLoader _documentTextLoader;
    private readonly IDocumentsNeuralModel _documentsNeuralModel;
    private readonly IFingerprintSearcher _fingerprintSearcher;

    public PlagiarismSearcher(IDocumentTextLoader documentTextLoader,
        IDocumentsNeuralModel documentsNeuralModel, 
        IFingerprintSearcher fingerprintSearcher)
    {
        _documentTextLoader = documentTextLoader;
        _documentsNeuralModel = documentsNeuralModel;
        _fingerprintSearcher = fingerprintSearcher;
    }
    
    public async Task<PlagiarismSearchResponse> SearchAsync(PlagiarismSearchRequest request)
    {
        var topFingerprints = await _fingerprintSearcher.SearchTopAsync(request.DocumentId, request.NDocuments);
        return new PlagiarismSearchResponse(null, topFingerprints);
    }
}