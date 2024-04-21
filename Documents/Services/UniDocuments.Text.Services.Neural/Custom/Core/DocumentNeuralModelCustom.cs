using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Services.Neural.Custom.Core.Models;
using UniDocuments.Text.Services.Neural.Custom.Core.Options;
using UniDocuments.Text.Services.Neural.Custom.Core.Tasks;

namespace UniDocuments.Text.Services.Neural.Custom.Core;

public abstract class DocumentNeuralModelCustom<T> : IDocumentsNeuralModel where T : CustomModelOptions, new()
{
    private readonly INeuralOptionsProvider<T> _optionsProvider;
    private readonly IDocumentMapper _documentMapper;

    private CustomManagedModel? _customManagedModel;

    protected DocumentNeuralModelCustom(INeuralOptionsProvider<T> optionsProvider, IDocumentMapper documentMapper)
    {
        _optionsProvider = optionsProvider;
        _documentMapper = documentMapper;
    }

    public string Name => _optionsProvider.GetOptions().Name;

    public Task SaveAsync(string path, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task LoadAsync(string path, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task TrainAsync(IDocumentsTrainDatasetSource source, CancellationToken cancellationToken)
    {
        var options = _optionsProvider.GetOptions();
        var input = new TrainCustomModelInput(source, options);
        _customManagedModel = await new PythonTaskTrainCustomModel(input);
    }

    public Task<List<ParagraphPlagiarismData>> FindSimilarAsync(
        UniDocument document, int topN, CancellationToken cancellationToken)
    {
        return Task.FromResult(new List<ParagraphPlagiarismData>());
    }
}