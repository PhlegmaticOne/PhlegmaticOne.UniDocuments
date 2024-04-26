using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Services.Neural.Doc2Vec.Tasks;
using UniDocuments.Text.Services.Neural.Keras.Core.Models;
using UniDocuments.Text.Services.Neural.Keras.Core.Options;
using UniDocuments.Text.Services.Neural.Keras.Core.Tasks;

namespace UniDocuments.Text.Services.Neural.Keras.Core;

public abstract class DocumentNeuralModelKeras<T> : IDocumentsNeuralModel where T : KerasModelOptions, new()
{
    private const string ModelNameFormat = "{0}.keras";
    
    private readonly INeuralOptionsProvider<T> _optionsProvider;
    private readonly IDocumentMapper _documentMapper;

    private KerasManagedModel? _customManagedModel;

    protected DocumentNeuralModelKeras(INeuralOptionsProvider<T> optionsProvider, IDocumentMapper documentMapper)
    {
        _optionsProvider = optionsProvider;
        _documentMapper = documentMapper;
    }

    public string Name => _optionsProvider.GetOptions().Name;

    public Task SaveAsync(string path, CancellationToken cancellationToken)
    {
        var savePath = GetModelPath(path);
        return _customManagedModel!.SaveAsync(savePath);
    }

    public async Task LoadAsync(string path, CancellationToken cancellationToken)
    {
        var loadPath = GetModelPath(path);
        _customManagedModel = await new PythonTaskLoadKerasModel(loadPath);
    }

    public async Task TrainAsync(IDocumentsTrainDatasetSource source, CancellationToken cancellationToken)
    {
        var options = _optionsProvider.GetOptions();
        var input = new TrainKerasModelInput(source, options);
        _customManagedModel = await new PythonTaskTrainKerasModel(input);
    }

    public async Task<List<ParagraphPlagiarismData>> FindSimilarAsync(
        UniDocument document, int topN, CancellationToken cancellationToken)
    {
        var options = _optionsProvider.GetOptions();
        var infer = await _customManagedModel!.InferDocumentAsync(document.Content!, topN, options);
        return MapResults(new[]{infer}, document.Id);
    }
    
    private string GetModelPath(string basePath)
    {
        var name = string.Format(ModelNameFormat, Name);
        return Path.Combine(basePath, name);
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