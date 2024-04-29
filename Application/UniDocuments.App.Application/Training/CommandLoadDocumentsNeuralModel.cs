using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Services.Neural;

namespace UniDocuments.App.Application.Training;

public class CommandLoadDocumentsNeuralModel : IOperationResultCommand
{
    public CommandLoadDocumentsNeuralModel(string savePath)
    {
        SavePath = savePath;
    }

    public string SavePath { get; }
}

public class CommandLoadDocumentsNeuralModelHandler : IOperationResultCommandHandler<CommandLoadDocumentsNeuralModel>
{
    private readonly IDocumentsNeuralModel _documentsNeuralModel;

    public CommandLoadDocumentsNeuralModelHandler(IDocumentsNeuralModel documentsNeuralModel)
    {
        _documentsNeuralModel = documentsNeuralModel;
    }
    
    public async Task<OperationResult> Handle(CommandLoadDocumentsNeuralModel request, CancellationToken cancellationToken)
    {
        try
        {
            await _documentsNeuralModel.LoadAsync(request.SavePath, cancellationToken);
            return OperationResult.Success;
        }
        catch(Exception e)
        {
            return OperationResult.Failed<string>(e.Message);
        }
    }
}