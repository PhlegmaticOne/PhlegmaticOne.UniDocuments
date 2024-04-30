using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Models;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Domain.Services.SavePath;
using UniDocuments.Text.Services.Neural.Doc2Vec.Models;
using UniDocuments.Text.Services.Neural.Doc2Vec.Options;
using UniDocuments.Text.Services.Neural.Doc2Vec.Tasks;

namespace UniDocuments.Text.Services.Neural.Doc2Vec;

public class DocumentNeuralModelDoc2Vec : IDocumentsNeuralModel
{
    private const string ModelNameFormat = "{0}.bin";

    private readonly INeuralOptionsProvider<Doc2VecOptions> _optionsProvider;
    private readonly ISavePathProvider _savePathProvider;

    private Doc2VecManagedModel? _doc2VecModel;

    public DocumentNeuralModelDoc2Vec(INeuralOptionsProvider<Doc2VecOptions> optionsProvider, ISavePathProvider savePathProvider)
    {
        _optionsProvider = optionsProvider;
        _savePathProvider = savePathProvider;
    }

    public bool IsLoaded { get; set; }
    public string Name => _optionsProvider.GetOptions().Name;

    public async Task LoadAsync(CancellationToken cancellationToken)
    {
        var loadPath = GetModelPath();
        _doc2VecModel = await new PythonTaskLoadDoc2VecModel(loadPath);
        IsLoaded = true;
    }

    public Task SaveAsync(CancellationToken cancellationToken)
    {
        var savePath = GetModelPath();
        return _doc2VecModel!.SaveAsync(savePath);
    }

    public async Task TrainAsync(IDocumentsTrainDatasetSource source, CancellationToken cancellationToken)
    {
        var options = _optionsProvider.GetOptions();
        var input = new TrainDoc2VecModelInput(source, options);
        _doc2VecModel = await new PythonTaskTrainDoc2VecModel(input);
        IsLoaded = true;
    }

    public async Task<InferVectorOutput[]> FindSimilarAsync(UniDocument document, int topN, CancellationToken cancellationToken)
    {
        var options = _optionsProvider.GetOptions();
        return await _doc2VecModel!.InferDocumentAsync(document.Content!, topN, options);
    }

    private string GetModelPath()
    {
        var basePath = _savePathProvider.SavePath;
        var name = string.Format(ModelNameFormat, Name);
        return Path.Combine(basePath, name);
    }
}