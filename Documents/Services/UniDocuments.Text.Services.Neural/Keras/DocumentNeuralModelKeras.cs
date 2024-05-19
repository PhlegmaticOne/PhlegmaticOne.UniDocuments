using System.Diagnostics;
using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Models;
using UniDocuments.Text.Domain.Services.Neural.Models.Inferring;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Domain.Services.Neural.Vocab;
using UniDocuments.Text.Domain.Services.SavePath;
using UniDocuments.Text.Services.Neural.Keras.Models;
using UniDocuments.Text.Services.Neural.Keras.Options;
using UniDocuments.Text.Services.Neural.Keras.Result;
using UniDocuments.Text.Services.Neural.Keras.Tasks;

namespace UniDocuments.Text.Services.Neural.Keras;

public class DocumentNeuralModelKeras : IDocumentsNeuralModel
{
    private const string ModelIsTrainingErrorMessage = "Model is training now";
    
    private readonly INeuralOptionsProvider<KerasModelOptions> _optionsProvider;
    private readonly IDocumentsVocabProvider _documentsVocabProvider;
    private readonly ISavePathProvider _savePathProvider;

    private KerasManagedModel? _customManagedModel;
    private bool _isTraining;

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

    public Task SaveAsync()
    {
        return _customManagedModel is null ? 
            Task.CompletedTask : 
            _customManagedModel.SaveAsync(_savePathProvider.SavePath, Name);
    }

    public async Task LoadAsync()
    {
        var vocab = await GetLoadedVocabAsync();
        var options = _optionsProvider.GetOptions();
        var input = new LoadKerasModelInput(_savePathProvider.SavePath, Name, vocab, options);
        _customManagedModel = await new PythonTaskLoadKerasModel(input);
        IsLoaded = true;
    }

    public async Task<NeuralModelTrainResult> TrainAsync(
        IDocumentsTrainDatasetSource source, NeuralTrainOptionsBase optionsBase)
    {
        var options = _optionsProvider.GetOptions().Merge((NeuralTrainOptionsKeras)optionsBase);

        if (_isTraining)
        {
            return GetResult(options, TimeSpan.Zero, ModelIsTrainingErrorMessage);
        }

        _isTraining = true;
        var timer = Stopwatch.StartNew();
        
        try
        {
            await _documentsVocabProvider.BuildAsync(source);
            var vocab = _documentsVocabProvider.GetVocab();
            var input = new TrainKerasModelInput(source, options, vocab);
            _customManagedModel = await new PythonTaskTrainKerasModel(input);
        }
        catch (Exception e)
        {
            return GetResult(options, timer.Elapsed, e.Message);
        }
        
        timer.Stop();
        IsLoaded = true;
        _isTraining = false;

        return GetResult(options, timer.Elapsed, null);
    }

    public Task<InferVectorOutput[]> FindSimilarAsync(PlagiarismSearchRequest request)
    {
        var options = _optionsProvider.GetOptions();
        return _customManagedModel!.InferDocumentAsync(request, options);
    }

    private async Task<object> GetLoadedVocabAsync()
    {
        if (!_documentsVocabProvider.IsLoaded)
        {
            await _documentsVocabProvider.LoadAsync();
        }
        
        return _documentsVocabProvider.GetVocab();
    }

    private NeuralModelTrainResult GetResult(KerasModelOptions options, TimeSpan time, string? errorMessage)
    {
        return new NeuralTrainResultKeras
        {
            Name = Name,
            Epochs = options.Epochs,
            EmbeddingSize = options.EmbeddingSize,
            TrainTime = time,
            LearningRate = (float)options.LearningRate,
            BatchSize = options.BatchSize,
            WindowSize = options.BatchSize,
            ErrorMessage = errorMessage
        };
    }
}