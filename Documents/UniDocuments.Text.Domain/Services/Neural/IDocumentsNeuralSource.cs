using UniDocuments.Text.Domain.Services.Common;

namespace UniDocuments.Text.Domain.Services.Neural;

public interface IDocumentsNeuralSource : IDisposable
{
    Task InitializeAsync();
    Task<RawDocument> GetNextDocumentAsync();
}