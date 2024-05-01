using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Providers.Neural;
using UniDocuments.Text.Domain.Services.Neural;

namespace UniDocuments.App.Application.Training;

public class CommandTrainDocumentsNeuralModel : IOperationResultCommand
{
    public string ModelName { get; set; } = null!;
    public bool IsRebuildVocab { get; set; }
}

public class CommandTrainDocumentsNeuralModelHandler : IOperationResultCommandHandler<CommandTrainDocumentsNeuralModel>
{
    private readonly IDocumentNeuralModelsProvider _neuralModelsProvider;
    private readonly IDocumentsVocabProvider _documentsVocabProvider;
    private readonly IDocumentsTrainDatasetSource _documentsTrainDatasetSource;

    public CommandTrainDocumentsNeuralModelHandler(
        IDocumentNeuralModelsProvider neuralModelsProvider,
        IDocumentsVocabProvider documentsVocabProvider,
        IDocumentsTrainDatasetSource documentsTrainDatasetSource)
    {
        _neuralModelsProvider = neuralModelsProvider;
        _documentsVocabProvider = documentsVocabProvider;
        _documentsTrainDatasetSource = documentsTrainDatasetSource;
    }
    
    public async Task<OperationResult> Handle(
        CommandTrainDocumentsNeuralModel request, CancellationToken cancellationToken)
    {
        try
        {
            var model = await _neuralModelsProvider.GetModelAsync(request.ModelName, false, cancellationToken);

            if (model is null)
            {
                return OperationResult.Failed("TrainModel.ModelNotFound");
            }
            
            if (request.IsRebuildVocab)
            {
                await _documentsVocabProvider.BuildAsync(cancellationToken);
            }
            
            await model.TrainAsync(_documentsTrainDatasetSource, cancellationToken);
            await model.SaveAsync(cancellationToken);
            return OperationResult.Success;
        }
        catch(Exception e)
        {
            return OperationResult.Failed("TrainDocument.InternalError", e.Message);
        }
    }
}