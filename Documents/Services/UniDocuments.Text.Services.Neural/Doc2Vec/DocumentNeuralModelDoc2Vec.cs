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
using UniDocuments.Text.Services.Neural.Doc2Vec.Tasks;

namespace UniDocuments.Text.Services.Neural.Doc2Vec;

public class DocumentNeuralModelDoc2Vec : IDocumentsNeuralModel
{
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

    public async Task LoadAsync(CancellationToken cancellationToken)
    {
        var input = new PythonModelPathData(_savePathProvider.SavePath, Name);
        _doc2VecModel = await new PythonTaskLoadDoc2VecModel(input);
        IsLoaded = true;
    }

    public Task SaveAsync(CancellationToken cancellationToken)
    {
        var input = new PythonModelPathData(_savePathProvider.SavePath, Name);
        return _doc2VecModel!.SaveAsync(input);
    }

    public async Task<NeuralModelTrainResult> TrainAsync(IDocumentsTrainDatasetSource source, CancellationToken cancellationToken)
    {
        var options = _optionsProvider.GetOptions();

        if (_isTraining)
        {
            return GetResult(options, TimeSpan.Zero);
        }
        
        _isTraining = true;
        var input = new TrainDoc2VecModelInput(source, options);
        var timer = Stopwatch.StartNew();
        _doc2VecModel = await new PythonTaskTrainDoc2VecModel(input);
        timer.Stop();
        IsLoaded = true;
        _isTraining = false;
        return GetResult(options, timer.Elapsed);
    }

    public Task<InferVectorOutput[]> FindSimilarAsync(PlagiarismSearchRequest request, CancellationToken cancellationToken)
    {
        var options = _optionsProvider.GetOptions();
        return _doc2VecModel!.InferDocumentAsync(request, options);
    }

    private NeuralModelTrainResult GetResult(Doc2VecOptions options, TimeSpan time)
    {
        return new NeuralModelTrainResult
        {
            Name = Name,
            Epochs = options.Epochs,
            EmbeddingSize = options.EmbeddingSize,
            TrainTime = time
        };
    }
}