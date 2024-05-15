using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Providers.Neural;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Models;
using UniDocuments.Text.Domain.Services.Neural.Options;

namespace UniDocuments.App.Application.Documents.Training;

public class CommandTrainDoc2VecModel : IOperationResultCommand
{
    public NeuralTrainOptionsBase Options { get; }

    public CommandTrainDoc2VecModel(NeuralTrainOptionsBase options)
    {
        Options = options;
    }
}

public class CommandTrainDoc2VecModelHandler : IOperationResultCommandHandler<CommandTrainDoc2VecModel>
{
    private const string TrainDocumentInternalError = "TrainDoc2Vec.InternalError";
    
    private readonly IDocumentNeuralModelsProvider _neuralModelsProvider;
    private readonly IDocumentsTrainDatasetSource _documentsTrainDatasetSource;
    private readonly ILogger<CommandTrainDoc2VecModelHandler> _logger;

    public CommandTrainDoc2VecModelHandler(
        IDocumentNeuralModelsProvider neuralModelsProvider,
        IDocumentsTrainDatasetSource documentsTrainDatasetSource,
        ILogger<CommandTrainDoc2VecModelHandler> logger)
    {
        _neuralModelsProvider = neuralModelsProvider;
        _documentsTrainDatasetSource = documentsTrainDatasetSource;
        _logger = logger;
    }
    
    public async Task<OperationResult> Handle(CommandTrainDoc2VecModel request, CancellationToken cancellationToken)
    {
        try
        {
            var model = await _neuralModelsProvider.GetModelAsync("doc2vec", false);
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