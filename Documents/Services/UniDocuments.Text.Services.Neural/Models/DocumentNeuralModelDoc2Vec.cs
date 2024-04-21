using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Requests;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Domain.Services.Neural.Requests;
using UniDocuments.Text.Domain.Services.StreamReading;
using UniDocuments.Text.Services.Neural.Models.Tasks;

namespace UniDocuments.Text.Services.Neural.Models;

public class DocumentNeuralModelDoc2Vec : IDocumentsNeuralModel
{
    private const string ModelName = "model.bin";

    private readonly IDocumentNeuralOptionsProvider _optionsProvider;
    private readonly IDocumentsStorage _documentsStorage;
    private readonly IStreamContentReader _streamContentReader;
    private readonly IDocumentMapper _documentMapper;
    
    private Doc2VecModel? _doc2VecModel;

    public DocumentNeuralModelDoc2Vec(
        IDocumentNeuralOptionsProvider optionsProvider, 
        IDocumentsStorage documentsStorage,
        IStreamContentReader streamContentReader,
        IDocumentMapper documentMapper)
    {
        _optionsProvider = optionsProvider;
        _documentsStorage = documentsStorage;
        _streamContentReader = streamContentReader;
        _documentMapper = documentMapper;
    }

    public async Task LoadAsync(string path, CancellationToken cancellationToken)
    {
        var loadPath = Path.Combine(path, ModelName);
        _doc2VecModel = await new PythonTaskLoadDoc2VecModel(loadPath);
    }

    public async Task TrainAsync(IDocumentsNeuralSource source, CancellationToken cancellationToken)
    {
        var options = _optionsProvider.GetOptions();
        var input = new TrainDoc2VecModelInput(source, options);
        _doc2VecModel = await new PythonTaskTrainDoc2VecModel(input);
    }

    public async Task<List<ParagraphPlagiarismData>> FindSimilarAsync(
        NeuralSearchPlagiarismRequest request, CancellationToken cancellationToken)
    {
        var options = _optionsProvider.GetOptions();
        var document = await _documentsStorage.LoadAsync(new DocumentLoadRequest(request.DocumentId), cancellationToken);
        var content = await _streamContentReader.ReadAsync(document.Stream!, cancellationToken);
        var inferOutputs = await _doc2VecModel!.InferDocumentAsync(content, request.TopN, options);
        return MapResults(inferOutputs, request.DocumentId);
    }

    public Task SaveAsync(string path, CancellationToken cancellationToken)
    {
        var resultPath = Path.Combine(path, ModelName);
        return _doc2VecModel!.SaveAsync(resultPath);
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