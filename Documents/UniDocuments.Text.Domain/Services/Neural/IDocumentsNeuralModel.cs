using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Services.Neural.Requests;

namespace UniDocuments.Text.Domain.Services.Neural;

public interface IDocumentsNeuralModel
{
    Task SaveAsync(string path, CancellationToken cancellationToken);
    Task LoadAsync(string path, CancellationToken cancellationToken);
    Task TrainAsync(IDocumentsNeuralSource source, CancellationToken cancellationToken);
    Task<List<ParagraphPlagiarismData>> FindSimilarAsync(NeuralSearchPlagiarismRequest request, CancellationToken cancellationToken);
}