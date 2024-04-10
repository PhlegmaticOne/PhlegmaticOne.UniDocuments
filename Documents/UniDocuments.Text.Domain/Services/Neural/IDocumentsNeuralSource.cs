using UniDocuments.Text.Domain.Shared;

namespace UniDocuments.Text.Domain.Services.Neural;

public interface IDocumentsNeuralSource : IDisposable
{
    Task InitializeAsync();
    Task<RawDocument> GetNextDocumentAsync();
}