using UniDocuments.Text.Domain.Services.Neural;

namespace UniDocuments.Text.Domain.Providers.Neural;

public class NeuralModelsProvider : INeuralModelsProvider
{
    private readonly Dictionary<string, IDocumentsNeuralModel> _neuralModels;
    
    public NeuralModelsProvider(IEnumerable<IDocumentsNeuralModel> neuralModels)
    {
        _neuralModels = neuralModels.ToDictionary(x => x.Name, x => x);
    }
    
    public async Task<IDocumentsNeuralModel?> GetNeuralModelAsync(string key, CancellationToken cancellationToken)
    {
        if (_neuralModels.TryGetValue(key, out var model) == false)
        {
            return null;
        }

        if (!model.IsLoaded)
        {
            await model.LoadAsync(cancellationToken);
        }

        return model;
    }
}