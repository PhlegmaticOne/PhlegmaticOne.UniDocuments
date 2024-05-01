using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Services.Neural.Doc2Vec.Options;
using UniDocuments.Text.Services.Neural.Keras.Options;

namespace UniDocuments.Text.Root.Builders;

public class DocumentNeuralInstallBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public DocumentNeuralInstallBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }
    
    public void UseNeuralModel<T>() where T : class, IDocumentsNeuralModel
    {
        _serviceCollection.AddSingleton<IDocumentsNeuralModel, T>();
    }
    
    public void UseVocabProvider<T>() where T : class, IDocumentsVocabProvider
    {
        _serviceCollection.AddSingleton<IDocumentsVocabProvider, T>();
    }

    public void UseTrainDatasetSource<T>() where T : class, IDocumentsTrainDatasetSource
    {
        _serviceCollection.AddSingleton<IDocumentsTrainDatasetSource, T>();
    }
    
    public void BindDoc2VecOptions(IConfiguration configuration, string sectionName)
    {
        var section = configuration.GetSection(sectionName);
        _serviceCollection.AddSingleton<INeuralOptionsProvider<Doc2VecOptions>, Doc2VecOptionsProvider>();
        _serviceCollection.Configure<Doc2VecOptions>(section);
    }

    public void BindKerasOptions(IConfiguration configuration, string sectionName)
    {
        var section = configuration.GetSection(sectionName);
        _serviceCollection.AddSingleton<INeuralOptionsProvider<KerasModelOptions>, KerasModelOptionsProvider>();
        _serviceCollection.Configure<KerasModelOptions>(section);
    }
}