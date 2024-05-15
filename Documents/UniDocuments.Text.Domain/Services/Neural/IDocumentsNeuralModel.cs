using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Services.Neural.Models;
using UniDocuments.Text.Domain.Services.Neural.Models.Inferring;
using UniDocuments.Text.Domain.Services.Neural.Options;

namespace UniDocuments.Text.Domain.Services.Neural;

public interface IDocumentsNeuralModel
{
    bool IsLoaded { get; }
    string Name { get; }
    Task LoadAsync();
    Task SaveAsync();
    Task<NeuralModelTrainResult> TrainAsync(IDocumentsTrainDatasetSource source, NeuralTrainOptionsBase options);
    Task<InferVectorOutput[]> FindSimilarAsync(PlagiarismSearchRequest request);
}