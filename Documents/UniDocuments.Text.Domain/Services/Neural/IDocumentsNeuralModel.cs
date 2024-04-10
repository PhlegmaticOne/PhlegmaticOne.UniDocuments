using UniDocuments.Text.Domain.Services.Neural.Requests;
using UniDocuments.Text.Domain.Services.Searching.Response;

namespace UniDocuments.Text.Domain.Services.Neural;

public interface IDocumentsNeuralModel
{
    Task SaveAsync(string path, CancellationToken cancellationToken);
    Task LoadAsync(string path, CancellationToken cancellationToken);
    Task TrainAsync(IDocumentsNeuralSource source, CancellationToken cancellationToken);
    Task<List<ParagraphPlagiarismData>> FindSimilarAsync(FindPlagiarismRequest request, CancellationToken cancellationToken);
}