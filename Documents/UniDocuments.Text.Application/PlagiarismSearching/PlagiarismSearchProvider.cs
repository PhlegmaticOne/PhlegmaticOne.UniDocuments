using UniDocuments.Text.Domain.Providers.Neural;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;
using UniDocuments.Text.Domain.Services.Neural.Models;

namespace UniDocuments.Text.Application.PlagiarismSearching;

public class PlagiarismSearchProvider : IPlagiarismSearchProvider
{
    private readonly IFingerprintSearcher _fingerprintSearcher;
    private readonly INeuralModelsProvider _neuralModelsProvider;
    private readonly IDocumentMapper _documentMapper;

    public PlagiarismSearchProvider(
        IFingerprintSearcher fingerprintSearcher,
        INeuralModelsProvider neuralModelsProvider,
        IDocumentMapper documentMapper)
    {
        _fingerprintSearcher = fingerprintSearcher;
        _neuralModelsProvider = neuralModelsProvider;
        _documentMapper = documentMapper;
    }
    
    public async Task<PlagiarismSearchResponse> SearchAsync(
        PlagiarismSearchRequest request, CancellationToken cancellationToken)
    {
        var documentId = request.Document.Id;
        var algorithm = request.AlgorithmData;
        var response = new PlagiarismSearchResponse();

        if (algorithm.UseFingerprint)
        {
            var topFingerprints = await _fingerprintSearcher
                .SearchTopAsync(documentId, request.NDocuments, cancellationToken);

            response.SuspiciousDocuments = topFingerprints;
        }

        var model = await _neuralModelsProvider.GetNeuralModelAsync(algorithm.NeuralModelType, cancellationToken);

        if (model is null)
        {
            return response;
        }
        
        var ingerOutputs = await model.FindSimilarAsync(request.Document, request.NDocuments, cancellationToken);
        var topParagraphs = MapResults(ingerOutputs, documentId);
        response.SuspiciousParagraphs = topParagraphs;
        return response;
    }
    
    private List<ParagraphPlagiarismData> MapResults(InferVectorOutput[] inferOutputs, Guid sourceId)
    {
        var result = new List<ParagraphPlagiarismData>();
        var sourceDocumentId = _documentMapper.GetDocumentId(sourceId);
        
        foreach (var inferOutput in inferOutputs)
        {
            var paragraphPlagiarism = new List<ParagraphSearchData>();

            foreach (var inferEntry in inferOutput.InferEntries)
            {
                var documentId = _documentMapper.GetDocumentIdFromGlobalParagraphId(inferEntry.ParagraphId);

                if (documentId == sourceDocumentId)
                {
                    continue;
                }
                
                var documentData = _documentMapper.GetDocumentData(documentId)!;
                    
                paragraphPlagiarism.Add(new ParagraphSearchData
                {
                    DocumentId = documentData.Id,
                    DocumentName = documentData.Name,
                    Similarity = inferEntry.Similarity,
                    Id = inferEntry.ParagraphId - documentData.GlobalFirstParagraphId
                });
            }

            result.Add(new ParagraphPlagiarismData(inferOutput.ParagraphId, paragraphPlagiarism));
        }

        return result;
    }
}