using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Providers.Neural;

namespace UniDocuments.App.Application.Documents.Training;

public class CommandLoadDocumentsNeuralModel : IOperationResultCommand
{
    public string ModelName { get; set; } = null!;
}

public class CommandLoadDocumentsNeuralModelHandler : IOperationResultCommandHandler<CommandLoadDocumentsNeuralModel>
{
    private const string LoadModelModelNotFound = "LoadModel.ModelNotFound";
    private const string LoadModelInternalError = "LoadModel.InternalError";

    private readonly IDocumentNeuralModelsProvider _neuralModelsProvider;
    private readonly ILogger<CommandLoadDocumentsNeuralModelHandler> _logger;

    public CommandLoadDocumentsNeuralModelHandler(
        IDocumentNeuralModelsProvider neuralModelsProvider,
        ILogger<CommandLoadDocumentsNeuralModelHandler> logger)
    {
        _neuralModelsProvider = neuralModelsProvider;
        _logger = logger;
    }
    
    public async Task<OperationResult> Handle(CommandLoadDocumentsNeuralModel request, CancellationToken cancellationToken)
    {
        try
        {
            var model = await _neuralModelsProvider.GetModelAsync(request.ModelName, true, cancellationToken);
            return model is null ? OperationResult.Failed(LoadModelModelNotFound) : OperationResult.Success;
        }
        catch(Exception e)
        {
            _logger.LogCritical(e, LoadModelInternalError);
            return OperationResult.Failed(LoadModelInternalError, e.Message);
        }
    }
}