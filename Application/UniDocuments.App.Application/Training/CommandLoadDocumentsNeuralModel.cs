using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Providers.Neural;

namespace UniDocuments.App.Application.Training;

public class CommandLoadDocumentsNeuralModel : IOperationResultCommand
{
    public string ModelName { get; set; } = null!;
}

public class CommandLoadDocumentsNeuralModelHandler : IOperationResultCommandHandler<CommandLoadDocumentsNeuralModel>
{
    private readonly IDocumentNeuralModelsProvider _neuralModelsProvider;

    public CommandLoadDocumentsNeuralModelHandler(IDocumentNeuralModelsProvider neuralModelsProvider)
    {
        _neuralModelsProvider = neuralModelsProvider;
    }
    
    public async Task<OperationResult> Handle(CommandLoadDocumentsNeuralModel request, CancellationToken cancellationToken)
    {
        try
        {
            var model = await _neuralModelsProvider.GetModelAsync(request.ModelName, true, cancellationToken);
            return model is null ? OperationResult.Failed("LoadModel.ModelNotFound") : OperationResult.Success;
        }
        catch(Exception e)
        {
            return OperationResult.Failed("LoadModel.InternalError", e.Message);
        }
    }
}