using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Providers.Neural;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Models;
using UniDocuments.Text.Domain.Services.Neural.Options;

namespace UniDocuments.App.Application.Documents.Training;

public class CommandTrainModel : IOperationResultCommand
{
    public NeuralTrainOptionsBase Options { get; }
    public string ModelName { get; }

    public CommandTrainModel(NeuralTrainOptionsBase options, string modelName)
    {
        Options = options;
        ModelName = modelName;
    }
}

public class CommandTrainModelHandler : IOperationResultCommandHandler<CommandTrainModel>
{
    private const string TrainDocumentInternalError = "TrainKeras.InternalError";
    
    private readonly IDocumentNeuralModelsProvider _neuralModelsProvider;
    private readonly IDocumentsTrainDatasetSource _documentsTrainDatasetSource;
    private readonly ILogger<CommandTrainModelHandler> _logger;

    public CommandTrainModelHandler(
        IDocumentNeuralModelsProvider neuralModelsProvider,
        IDocumentsTrainDatasetSource documentsTrainDatasetSource,
        ILogger<CommandTrainModelHandler> logger)
    {
        _neuralModelsProvider = neuralModelsProvider;
        _documentsTrainDatasetSource = documentsTrainDatasetSource;
        _logger = logger;
    }
    
    public async Task<OperationResult> Handle(CommandTrainModel request, CancellationToken cancellationToken)
    {
        try
        {
            var model = await _neuralModelsProvider.GetModelAsync(request.ModelName, false);
            var result = await model!.TrainAsync(_documentsTrainDatasetSource, request.Options);

            if (result.IsError())
            {
                return OperationResult.Failed<NeuralModelTrainResult>(TrainDocumentInternalError, result);
            }
            
            await model.SaveAsync();
            return OperationResult.Successful(result);
        }
        catch(Exception e)
        {
            _logger.LogCritical(e, TrainDocumentInternalError);
            return OperationResult.Failed(TrainDocumentInternalError, e.Message);
        }
    }
}