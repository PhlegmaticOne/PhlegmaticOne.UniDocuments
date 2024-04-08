namespace UniDocuments.Text.Domain.Services.Neural;

public interface IDocumentsNeuralModel
{
    Task SaveAsync(string path);
    Task LoadAsync(string path);
    Task TrainAsync(IDocumentsNeuralSource source);
    Task<string> FindSimilarAsync(string text);
}