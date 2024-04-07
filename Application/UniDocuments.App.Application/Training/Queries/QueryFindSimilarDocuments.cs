using MediatR;
using UniDocuments.Text.Domain.Services.Neural;

namespace UniDocuments.App.Application.Training.Queries;

public class QueryFindSimilarDocuments : IRequest<string>
{
    public QueryFindSimilarDocuments(string text)
    {
        Text = text;
    }

    public string Text { get; }
}

public class QueryFindSimilarDocumentsHandler : IRequestHandler<QueryFindSimilarDocuments, string>
{
    private readonly IDocumentsNeuralModel _documentsNeuralModel;

    public QueryFindSimilarDocumentsHandler(IDocumentsNeuralModel documentsNeuralModel)
    {
        _documentsNeuralModel = documentsNeuralModel;
    }
    
    public Task<string> Handle(QueryFindSimilarDocuments request, CancellationToken cancellationToken)
    {
        return _documentsNeuralModel.FindSimilarAsync(request.Text);
    }
}