using UniDocuments.Text.Domain.Providers.Neural;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.DocumentMapping.Models;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Models;

namespace UniDocuments.Text.Services.Neural;

public class NeuralNetworkPlagiarismSearcher : INeuralNetworkPlagiarismSearcher
{
    private readonly IDocumentNeuralModelsProvider _neuralModelsProvider;
    private readonly IDocumentMapper _documentMapper;

    public NeuralNetworkPlagiarismSearcher(IDocumentNeuralModelsProvider neuralModelsProvider, IDocumentMapper documentMapper)
    {
        _neuralModelsProvider = neuralModelsProvider;
        _documentMapper = documentMapper;
    }

    public async Task<List<ParagraphPlagiarismData>> SearchAsync(
        PlagiarismSearchRequest request, CancellationToken cancellationToken)
    {
        var model = await _neuralModelsProvider.GetModelAsync(request.ModelName, true, cancellationToken);

        if (model is null)
        {
            return new List<ParagraphPlagiarismData>();
        }
        
        var ingerOutputs = await model.FindSimilarAsync(request.Document, request.NDocuments, cancellationToken);
        return MapResults(ingerOutputs, request.Document.Id);
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
            Id = inferEntry.ParagraphId
        };
    }

    private static ParagraphSearchData CreateKnown(DocumentGlobalMapData documentData, InferVectorEntry inferEntry)
    {
        return new ParagraphSearchData
        {
            DocumentId = documentData.Id,
            Similarity = inferEntry.Similarity,
            Id = documentData.GetLocalParagraphId(inferEntry.ParagraphId)
        };
    }
}