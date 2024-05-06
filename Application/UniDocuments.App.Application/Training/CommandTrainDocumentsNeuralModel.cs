using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Providers.Neural;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Vocab;

namespace UniDocuments.App.Application.Training;

public class CommandTrainDocumentsNeuralModel : IOperationResultCommand
{
    public string ModelName { get; set; } = null!;
    public bool IsRebuildVocab { get; set; }
}

public class CommandTrainDocumentsNeuralModelHandler : IOperationResultCommandHandler<CommandTrainDocumentsNeuralModel>
{
    private const string TrainModelModelNotFound = "TrainModel.ModelNotFound";
    private const string TrainDocumentInternalError = "TrainDocument.InternalError";
    
    private readonly IDocumentNeuralModelsProvider _neuralModelsProvider;
    private readonly IDocumentsVocabProvider _documentsVocabProvider;
    private readonly IDocumentsTrainDatasetSource _documentsTrainDatasetSource;
    private readonly IDocumentsTrainDatasetSource _source;
    private readonly ILogger<CommandTrainDocumentsNeuralModelHandler> _logger;

    public CommandTrainDocumentsNeuralModelHandler(
        IDocumentNeuralModelsProvider neuralModelsProvider,
        IDocumentsVocabProvider documentsVocabProvider,
        IDocumentsTrainDatasetSource documentsTrainDatasetSource,
        IDocumentsTrainDatasetSource source,
        ILogger<CommandTrainDocumentsNeuralModelHandler> logger)
    {
        _neuralModelsProvider = neuralModelsProvider;
        _documentsVocabProvider = documentsVocabProvider;
        _documentsTrainDatasetSource = documentsTrainDatasetSource;
        _source = source;
        _logger = logger;
    }
    
    public async Task<OperationResult> Handle(
        CommandTrainDocumentsNeuralModel request, CancellationToken cancellationToken)
    {
        try
        {
            var model = await _neuralModelsProvider.GetModelAsync(request.ModelName, false, cancellationToken);

            if (model is null)
            {
                return OperationResult.Failed(TrainModelModelNotFound);
            }
            
            if (request.IsRebuildVocab)
            {
                await _documentsVocabProvider.BuildAsync(_source, cancellationToken);
            }
            
            var result = await model.TrainAsync(_documentsTrainDatasetSource, cancellationToken);
            await model.SaveAsync(cancellationToken);
            return OperationResult.Successful(result);
        }
        catch(Exception e)
        {
            _logger.LogCritical(e, TrainDocumentInternalError);
            return OperationResult.Failed(TrainDocumentInternalError, e.Message);
        }
    }
}