namespace UniDocuments.Text.Domain.Services.Neural;

public interface IDocumentsNeuralSource : IDisposable
{
    Task InitializeAsync();
    Task<DocumentNeuralViewModel> GetNextDocumentAsync();
}