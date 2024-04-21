using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

namespace UniDocuments.Text.Domain.Services.Neural;

public interface IDocumentsNeuralModel
{
    string Name { get; }
    Task LoadAsync(string path, CancellationToken cancellationToken);
    Task SaveAsync(string path, CancellationToken cancellationToken);
    Task TrainAsync(IDocumentsTrainDatasetSource source, CancellationToken cancellationToken);
    Task<List<ParagraphPlagiarismData>> FindSimilarAsync(UniDocument document, int topN, CancellationToken cancellationToken);
}