using System.Diagnostics;
using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Models;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Domain.Services.Neural.Vocab;
using UniDocuments.Text.Domain.Services.SavePath;
using UniDocuments.Text.Services.Neural.Keras.Models;
using UniDocuments.Text.Services.Neural.Keras.Options;
using UniDocuments.Text.Services.Neural.Keras.Tasks;

namespace UniDocuments.Text.Services.Neural.Keras;

public class DocumentNeuralModelKeras : IDocumentsNeuralModel
{
    private readonly INeuralOptionsProvider<KerasModelOptions> _optionsProvider;
    private readonly IDocumentsVocabProvider _documentsVocabProvider;
    private readonly ISavePathProvider _savePathProvider;

    private KerasManagedModel? _customManagedModel;

    public DocumentNeuralModelKeras(
        INeuralOptionsProvider<KerasModelOptions> optionsProvider,
        IDocumentsVocabProvider documentsVocabProvider,
        ISavePathProvider savePathProvider)
    {
        _optionsProvider = optionsProvider;
        _documentsVocabProvider = documentsVocabProvider;
        _savePathProvider = savePathProvider;
    }

    public bool IsLoaded { get; private set; }
    public string Name => _optionsProvider.GetOptions().Name;

    public Task SaveAsync(CancellationToken cancellationToken)
    {
        return _customManagedModel!.SaveAsync(_savePathProvider.SavePath, Name);
    }

    public async Task LoadAsync(CancellationToken cancellationToken)
    {
        var vocab = await GetLoadedVocabAsync(cancellationToken);
        var options = _optionsProvider.GetOptions();
        var input = new LoadKerasModelInput(_savePathProvider.SavePath, Name, vocab, options);
        _customManagedModel = await new PythonTaskLoadKerasModel(input);
        IsLoaded = true;
    }

    public async Task<NeuralModelTrainResult> TrainAsync(IDocumentsTrainDatasetSource source, CancellationToken cancellationToken)
    {
        var options = _optionsProvider.GetOptions();
        var vocab = _documentsVocabProvider.GetVocab();
        var input = new TrainKerasModelInput(source, options, vocab);
        var timer = Stopwatch.StartNew();
        _customManagedModel = await new PythonTaskTrainKerasModel(input);
        timer.Stop();
        IsLoaded = true;

        return new NeuralModelTrainResult
        {
            Name = Name,
            Epochs = options.Epochs,
            EmbeddingSize = options.EmbeddingSize,
            TrainTime = timer.Elapsed
        };
    }

    public Task<InferVectorOutput[]> FindSimilarAsync(PlagiarismSearchRequest request, CancellationToken cancellationToken)
    {
        var options = _optionsProvider.GetOptions();
        return _customManagedModel!.InferDocumentAsync(request, options);
    }

    private async Task<object> GetLoadedVocabAsync(CancellationToken cancellationToken)
    {
        if (!_documentsVocabProvider.IsLoaded)
        {
            await _documentsVocabProvider.LoadAsync(cancellationToken);
        }
        
        return _documentsVocabProvider.GetVocab();
    }
}