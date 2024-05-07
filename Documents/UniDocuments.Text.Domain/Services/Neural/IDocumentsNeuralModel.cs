using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Services.Neural.Models;
using UniDocuments.Text.Domain.Services.Neural.Models.Inferring;

namespace UniDocuments.Text.Domain.Services.Neural;

public interface IDocumentsNeuralModel
{
    bool IsLoaded { get; }
    string Name { get; }
    Task LoadAsync(CancellationToken cancellationToken);
    Task SaveAsync(CancellationToken cancellationToken);
    Task<NeuralModelTrainResult> TrainAsync(IDocumentsTrainDatasetSource source, CancellationToken cancellationToken);
    Task<InferVectorOutput[]> FindSimilarAsync(PlagiarismSearchRequest request, CancellationToken cancellationToken);
}