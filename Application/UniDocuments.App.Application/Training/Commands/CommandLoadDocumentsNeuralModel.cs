using MediatR;
using UniDocuments.Text.Domain.Services.Neural;

namespace UniDocuments.App.Application.Training.Commands;

public class CommandLoadDocumentsNeuralModel : IRequest
{
    public CommandLoadDocumentsNeuralModel(string savePath)
    {
        SavePath = savePath;
    }

    public string SavePath { get; }
}

public class CommandLoadDocumentsNeuralModelHandler : IRequestHandler<CommandLoadDocumentsNeuralModel>
{
    private readonly IDocumentsNeuralModel _documentsNeuralModel;

    public CommandLoadDocumentsNeuralModelHandler(IDocumentsNeuralModel documentsNeuralModel)
    {
        _documentsNeuralModel = documentsNeuralModel;
    }
    
    public Task Handle(CommandLoadDocumentsNeuralModel request, CancellationToken cancellationToken)
    {
        return _documentsNeuralModel.LoadAsync(request.SavePath);
    }
}