using MediatR;
using UniDocuments.Text.Domain.Services.Neural;

namespace UniDocuments.App.Application.Training.Commands;

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
    private readonly IDocumentsNeuralModelSource _documentsNeuralModelSource;

    public CommandTrainDocumentsNeuralModelHandler(
        IDocumentsNeuralModel documentsNeuralModel,
        IDocumentsNeuralModelSource documentsNeuralModelSource)
    {
        _documentsNeuralModel = documentsNeuralModel;
        _documentsNeuralModelSource = documentsNeuralModelSource;
    }
    
    public async Task Handle(CommandTrainDocumentsNeuralModel request, CancellationToken cancellationToken)
    {
        await _documentsNeuralModel.TrainAsync(_documentsNeuralModelSource);
        await _documentsNeuralModel.SaveAsync(request.SavePath);
    }
}