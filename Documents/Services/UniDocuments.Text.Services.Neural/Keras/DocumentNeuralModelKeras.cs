using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Models;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Domain.Services.SavePath;
using UniDocuments.Text.Services.Neural.Keras.Models;
using UniDocuments.Text.Services.Neural.Keras.Options;
using UniDocuments.Text.Services.Neural.Keras.Tasks;

namespace UniDocuments.Text.Services.Neural.Keras;

public class DocumentNeuralModelKeras : IDocumentsNeuralModel
{
    private const string ModelNameFormat = "{0}.keras";
    
    private readonly INeuralOptionsProvider<KerasModelOptions> _optionsProvider;
    private readonly ISavePathProvider _savePathProvider;

    private KerasManagedModel? _customManagedModel;

    public DocumentNeuralModelKeras(INeuralOptionsProvider<KerasModelOptions> optionsProvider, ISavePathProvider savePathProvider)
    {
        _optionsProvider = optionsProvider;
        _savePathProvider = savePathProvider;
    }

    public bool IsLoaded { get; private set; }
    public string Name => _optionsProvider.GetOptions().Name;

    public Task SaveAsync(CancellationToken cancellationToken)
    {
        var savePath = GetModelPath();
        return _customManagedModel!.SaveAsync(savePath);
    }

    public async Task LoadAsync(CancellationToken cancellationToken)
    {
        var loadPath = GetModelPath();
        _customManagedModel = await new PythonTaskLoadKerasModel(loadPath);
        IsLoaded = true;
    }

    public async Task TrainAsync(IDocumentsTrainDatasetSource source, CancellationToken cancellationToken)
    {
        var options = _optionsProvider.GetOptions();
        var input = new TrainKerasModelInput(source, options);
        _customManagedModel = await new PythonTaskTrainKerasModel(input);
        IsLoaded = true;
    }

    public async Task<InferVectorOutput[]> FindSimilarAsync(
        UniDocument document, int topN, CancellationToken cancellationToken)
    {
        var options = _optionsProvider.GetOptions();
        return await _customManagedModel!.InferDocumentAsync(document.Content!, topN, options);
    }
    
    private string GetModelPath()
    {
        var basePath = _savePathProvider.SavePath;
        var name = string.Format(ModelNameFormat, Name);
        return Path.Combine(basePath, name);
    }
}