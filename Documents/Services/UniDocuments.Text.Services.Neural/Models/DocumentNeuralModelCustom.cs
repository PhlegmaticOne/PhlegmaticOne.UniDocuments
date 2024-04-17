using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Requests;

namespace UniDocuments.Text.Services.Neural.Models;

public class DocumentNeuralModelCustom : IDocumentsNeuralModel
{
    public Task SaveAsync(string path, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task LoadAsync(string path, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task TrainAsync(IDocumentsNeuralSource source, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<ParagraphPlagiarismData>> FindSimilarAsync(NeuralSearchPlagiarismRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}