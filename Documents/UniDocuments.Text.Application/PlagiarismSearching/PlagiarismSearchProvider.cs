using UniDocuments.Text.Domain.Providers.Neural;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.DocumentMapping.Models;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;
using UniDocuments.Text.Domain.Services.Neural.Models;

namespace UniDocuments.Text.Application.PlagiarismSearching;

public class PlagiarismSearchProvider : IPlagiarismSearchProvider
{
    private readonly IFingerprintSearcher _fingerprintSearcher;
    private readonly IDocumentNeuralModelsProvider _documentNeuralModelsProvider;
    private readonly IDocumentMapper _documentMapper;

    public PlagiarismSearchProvider(
        IFingerprintSearcher fingerprintSearcher,
        IDocumentNeuralModelsProvider documentNeuralModelsProvider,
        IDocumentMapper documentMapper)
    {
        _fingerprintSearcher = fingerprintSearcher;
        _documentNeuralModelsProvider = documentNeuralModelsProvider;
        _documentMapper = documentMapper;
    }
    
    public async Task<PlagiarismSearchResponseDocument> SearchAsync(
        PlagiarismSearchRequest request, CancellationToken cancellationToken)
    {
        var documentId = request.Document.Id;
        var algorithm = request.AlgorithmData;
        var response = new PlagiarismSearchResponseDocument();

        if (algorithm.UseFingerprint)
        {
            var topFingerprints = await _fingerprintSearcher
                .SearchTopAsync(documentId, request.NDocuments, cancellationToken);

            response.SuspiciousDocuments = topFingerprints;
        }

        var model = await _documentNeuralModelsProvider.GetModelAsync(algorithm.ModelName, true, cancellationToken);

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
                
                var documentData = _documentMapper.GetDocumentData(documentId);
                var resultData = documentData is null ? CreateUnknown(inferEntry) : CreateKnown(documentData, inferEntry);
                paragraphPlagiarism.Add(resultData);
            }

            result.Add(new ParagraphPlagiarismData(inferOutput.ParagraphId, paragraphPlagiarism));
        }

        return result;
    }

    private static ParagraphSearchData CreateUnknown(InferVectorEntry inferEntry)
    {
        return new ParagraphSearchData
        {
            DocumentId = Guid.Empty,
            Similarity = inferEntry.Similarity,
            DocumentName = string.Empty,
            Id = inferEntry.ParagraphId
        };
    }

    private static ParagraphSearchData CreateKnown(DocumentGlobalMapData documentData, InferVectorEntry inferEntry)
    {
        return new ParagraphSearchData
        {
            DocumentId = documentData.Id,
            DocumentName = documentData.Name,
            Similarity = inferEntry.Similarity,
            Id = documentData.GetLocalParagraphId(inferEntry.ParagraphId)
        };
    }
}