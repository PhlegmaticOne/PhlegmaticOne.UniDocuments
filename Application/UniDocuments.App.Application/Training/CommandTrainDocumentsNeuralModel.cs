using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Services.Neural;

namespace UniDocuments.App.Application.Training;

public class CommandTrainDocumentsNeuralModel : IOperationResultCommand { }

public class CommandTrainDocumentsNeuralModelHandler : IOperationResultCommandHandler<CommandTrainDocumentsNeuralModel>
{
    private readonly IDocumentsNeuralModel _documentsNeuralModel;
    private readonly IDocumentsTrainDatasetSource _documentsTrainDatasetSource;

    public CommandTrainDocumentsNeuralModelHandler(
        IDocumentsNeuralModel documentsNeuralModel,
        IDocumentsTrainDatasetSource documentsTrainDatasetSource)
    {
        _documentsNeuralModel = documentsNeuralModel;
        _documentsTrainDatasetSource = documentsTrainDatasetSource;
    }
    
    public async Task<OperationResult> Handle(
        CommandTrainDocumentsNeuralModel request, CancellationToken cancellationToken)
    {
        try
        {
            await _documentsNeuralModel.TrainAsync(_documentsTrainDatasetSource, cancellationToken);
            await _documentsNeuralModel.SaveAsync(cancellationToken);
            return OperationResult.Success;
        }
        catch(Exception e)
        {
            return OperationResult.Failed("TrainDocument.InternalError", e.Message);
        }
    }
}