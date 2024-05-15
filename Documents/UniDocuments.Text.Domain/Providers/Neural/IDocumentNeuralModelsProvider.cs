using UniDocuments.Text.Domain.Services.Neural;

namespace UniDocuments.Text.Domain.Providers.Neural;

public interface IDocumentNeuralModelsProvider
{
    Task<IDocumentsNeuralModel?> GetModelAsync(string key, bool loadIfNotLoaded);
    Task LoadModelsAsync();
}