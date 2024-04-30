using UniDocuments.Text.Domain.Services.Neural.Models;

namespace UniDocuments.Text.Domain.Services.Neural;

public interface IDocumentsNeuralModel
{
    bool IsLoaded { get; }
    string Name { get; }
    Task LoadAsync(CancellationToken cancellationToken);
    Task SaveAsync(CancellationToken cancellationToken);
    Task TrainAsync(IDocumentsTrainDatasetSource source, CancellationToken cancellationToken);
    Task<InferVectorOutput[]> FindSimilarAsync(UniDocument document, int topN, CancellationToken cancellationToken);
}