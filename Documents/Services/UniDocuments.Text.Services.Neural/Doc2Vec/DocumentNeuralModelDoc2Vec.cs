using System.Diagnostics;
using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Models;
using UniDocuments.Text.Domain.Services.Neural.Models.Inferring;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Domain.Services.SavePath;
using UniDocuments.Text.Services.Neural.Common;
using UniDocuments.Text.Services.Neural.Doc2Vec.Models;
using UniDocuments.Text.Services.Neural.Doc2Vec.Options;
using UniDocuments.Text.Services.Neural.Doc2Vec.Result;
using UniDocuments.Text.Services.Neural.Doc2Vec.Tasks;

namespace UniDocuments.Text.Services.Neural.Doc2Vec;

public class DocumentNeuralModelDoc2Vec : IDocumentsNeuralModel
{
    private const string ModelIsTrainingErrorMessage = "Model is training now";
    
    private readonly INeuralOptionsProvider<Doc2VecOptions> _optionsProvider;
    private readonly ISavePathProvider _savePathProvider;

    private Doc2VecManagedModel? _doc2VecModel;
    private bool _isTraining;

    public DocumentNeuralModelDoc2Vec(
        INeuralOptionsProvider<Doc2VecOptions> optionsProvider,
        ISavePathProvider savePathProvider)
    {
        _optionsProvider = optionsProvider;
        _savePathProvider = savePathProvider;
    }

    public bool IsLoaded { get; private set; }
    public string Name => _optionsProvider.GetOptions().Name;

    public async Task LoadAsync()
    {
        var input = new PythonModelPathData(_savePathProvider.SavePath, Name);
        _doc2VecModel = await new PythonTaskLoadDoc2VecModel(input);
        IsLoaded = true;
    }

    public Task SaveAsync()
    {
        if (_doc2VecModel is null)
        {
            return Task.CompletedTask;
        }
        
        var input = new PythonModelPathData(_savePathProvider.SavePath, Name);
        return _doc2VecModel.SaveAsync(input);
    }

    public async Task<NeuralModelTrainResult> TrainAsync(
        IDocumentsTrainDatasetSource source, NeuralTrainOptionsBase optionsBase)
    {
        var merged = _optionsProvider.GetOptions().Merge((NeuralTrainOptionsDoc2Vec)optionsBase);

        if (_isTraining)
        {
            return GetResult(merged, TimeSpan.Zero, ModelIsTrainingErrorMessage);
        }
        
        _isTraining = true;
        var input = new TrainDoc2VecModelInput(source, merged);
        var timer = Stopwatch.StartNew();
        
        try
        {
            _doc2VecModel = await new PythonTaskTrainDoc2VecModel(input);
        }
        catch (Exception e)
        {
            return GetResult(merged, timer.Elapsed, e.Message);
        }
        
        timer.Stop();
        IsLoaded = true;
        _isTraining = false;
        return GetResult(merged, timer.Elapsed, null);
    }

    public Task<InferVectorOutput[]> FindSimilarAsync(PlagiarismSearchRequest request)
    {
        var options = _optionsProvider.GetOptions();
        return _doc2VecModel!.InferDocumentAsync(request, options);
    }

    public bool IsSuspicious(double similarity)
    {
        return similarity >= _optionsProvider.GetOptions().BaseLine;
    }

    private NeuralModelTrainResult GetResult(Doc2VecOptions options, TimeSpan time, string? errorMessage)
    {
        return new NeuralTrainResultDoc2Vec
        {
            Name = Name,
            Epochs = options.Epochs,
            EmbeddingSize = options.EmbeddingSize,
            TrainTime = time,
            WorkersCount = options.WorkersCount,
            MinWordsCount = options.MinWordsCount,
            MinAlpha = options.MinAlpha,
            Dm = options.Dm,
            LearningRate = options.Alpha,
            ErrorMessage = errorMessage
        };
    }
}