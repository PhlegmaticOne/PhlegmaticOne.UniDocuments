using MediatR;
using UniDocuments.Text.Domain.Services.Neural;

namespace UniDocuments.App.Application.Training;

public class CommandTrainDocumentsNeuralModel : IRequest
{
    public CommandTrainDocumentsNeuralModel(string savePath)
    {
        SavePath = savePath;
    }

    public string SavePath { get; }
}

public class CommandTrainDocumentsNeuralModelHandler : IRequestHandler<CommandTrainDocumentsNeuralModel>
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
    
    public async Task Handle(CommandTrainDocumentsNeuralModel request, CancellationToken cancellationToken)
    {
        await _documentsNeuralModel.TrainAsync(_documentsTrainDatasetSource, cancellationToken);
        await _documentsNeuralModel.SaveAsync(request.SavePath, cancellationToken);
    }
}