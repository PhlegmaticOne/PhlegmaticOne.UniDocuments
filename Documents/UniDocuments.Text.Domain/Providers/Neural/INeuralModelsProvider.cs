using UniDocuments.Text.Domain.Services.Neural;

namespace UniDocuments.Text.Domain.Providers.Neural;

public interface INeuralModelsProvider
{
    Task<IDocumentsNeuralModel?> GetNeuralModelAsync(string key, CancellationToken cancellationToken);
}