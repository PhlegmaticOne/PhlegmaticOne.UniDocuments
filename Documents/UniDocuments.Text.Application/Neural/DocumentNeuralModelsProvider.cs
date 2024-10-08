﻿using UniDocuments.Text.Domain.Providers.Neural;
using UniDocuments.Text.Domain.Services.Neural;

namespace UniDocuments.Text.Application.Neural;

public class DocumentNeuralModelsProvider : IDocumentNeuralModelsProvider
{
    private readonly Dictionary<string, IDocumentsNeuralModel> _neuralModels;
    
    public DocumentNeuralModelsProvider(IEnumerable<IDocumentsNeuralModel> neuralModels)
    {
        _neuralModels = neuralModels.ToDictionary(x => x.Name.ToLower(), x => x);
    }
    
    public async Task<IDocumentsNeuralModel?> GetModelAsync(string key, bool loadIfNotLoaded)
    {
        if (_neuralModels.TryGetValue(key.ToLower(), out var model) == false)
        {
            return null;
        }

        if (!model.IsLoaded && loadIfNotLoaded)
        {
            await model.LoadAsync();
        }

        return model;
    }

    public async Task LoadModelsAsync()
    {
        foreach (var neuralModel in _neuralModels)
        {
            await neuralModel.Value.LoadAsync();
        }
    }
}