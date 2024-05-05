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

public class CommandTrainDocumentsNeuralModelHandler : 
    IOperationResultCommandHandler<CommandTrainDocumentsNeuralModel>
{
    private readonly IDocumentNeuralModelsProvider _neuralModelsProvider;
    private readonly IDocumentsVocabProvider _documentsVocabProvider;
    private readonly IDocumentsTrainDatasetSource _documentsTrainDatasetSource;
    private readonly IDocumentsTrainDatasetSource _source;

    public CommandTrainDocumentsNeuralModelHandler(
        IDocumentNeuralModelsProvider neuralModelsProvider,
        IDocumentsVocabProvider documentsVocabProvider,
        IDocumentsTrainDatasetSource documentsTrainDatasetSource,
        IDocumentsTrainDatasetSource source)
    {
        _neuralModelsProvider = neuralModelsProvider;
        _documentsVocabProvider = documentsVocabProvider;
        _documentsTrainDatasetSource = documentsTrainDatasetSource;
        _source = source;
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
                await _documentsVocabProvider.BuildAsync(_source, cancellationToken);
            }
            
            var result = await model.TrainAsync(_documentsTrainDatasetSource, cancellationToken);
            await model.SaveAsync(cancellationToken);
            return OperationResult.Successful(result);
        }
        catch(Exception e)
        {
            return OperationResult.Failed("TrainDocument.InternalError", e.Message);
        }
    }
}