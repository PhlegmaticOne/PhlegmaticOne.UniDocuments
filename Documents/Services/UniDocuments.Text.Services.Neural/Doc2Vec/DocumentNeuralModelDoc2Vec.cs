using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Services.Neural.Doc2Vec.Models;
using UniDocuments.Text.Services.Neural.Doc2Vec.Options;
using UniDocuments.Text.Services.Neural.Doc2Vec.Tasks;

namespace UniDocuments.Text.Services.Neural.Doc2Vec;

public class DocumentNeuralModelDoc2Vec : IDocumentsNeuralModel
{
    private const string ModelNameFormat = "{0}.bin";

    private readonly INeuralOptionsProvider<Doc2VecOptions> _optionsProvider;
    private readonly IDocumentMapper _documentMapper;
    
    private Doc2VecManagedModel? _doc2VecModel;

    public DocumentNeuralModelDoc2Vec(INeuralOptionsProvider<Doc2VecOptions> optionsProvider, IDocumentMapper documentMapper)
    {
        _optionsProvider = optionsProvider;
        _documentMapper = documentMapper;
    }

    public string Name => _optionsProvider.GetOptions().Name;

    public async Task LoadAsync(string path, CancellationToken cancellationToken)
    {
        var loadPath = GetModelPath(path);
        _doc2VecModel = await new PythonTaskLoadDoc2VecModel(loadPath);
    }

    public async Task TrainAsync(IDocumentsTrainDatasetSource source, CancellationToken cancellationToken)
    {
        var options = _optionsProvider.GetOptions();
        var input = new TrainDoc2VecModelInput(source, options);
        _doc2VecModel = await new PythonTaskTrainDoc2VecModel(input);
    }

    public async Task<List<ParagraphPlagiarismData>> FindSimilarAsync(UniDocument document, int topN, CancellationToken cancellationToken)
    {
        var options = _optionsProvider.GetOptions();
        var inferOutputs = await _doc2VecModel!.InferDocumentAsync(document.Content!, topN, options);
        return MapResults(inferOutputs, document.Id);
    }
    
    public Task SaveAsync(string path, CancellationToken cancellationToken)
    {
        var savePath = GetModelPath(path);
        return _doc2VecModel!.SaveAsync(savePath);
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

    private string GetModelPath(string basePath)
    {
        var name = string.Format(ModelNameFormat, Name);
        return Path.Combine(basePath, name);
    }
}