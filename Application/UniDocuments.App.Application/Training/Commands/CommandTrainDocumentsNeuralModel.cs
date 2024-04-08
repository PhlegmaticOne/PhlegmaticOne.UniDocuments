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
    private readonly IDocumentsNeuralSource _documentsNeuralSource;

    public CommandTrainDocumentsNeuralModelHandler(
        IDocumentsNeuralModel documentsNeuralModel,
        IDocumentsNeuralSource documentsNeuralSource)
    {
        _documentsNeuralModel = documentsNeuralModel;
        _documentsNeuralSource = documentsNeuralSource;
    }
    
    public async Task Handle(CommandTrainDocumentsNeuralModel request, CancellationToken cancellationToken)
    {
        await _documentsNeuralModel.TrainAsync(_documentsNeuralSource);
        await _documentsNeuralModel.SaveAsync(request.SavePath);
    }
}