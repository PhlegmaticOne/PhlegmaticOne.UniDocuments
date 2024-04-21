using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Options;

namespace UniDocuments.Text.Root.Builders;

public class DocumentNeuralInstallBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public DocumentNeuralInstallBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public void UseTrainDatasetSource<T>() where T : class, IDocumentsTrainDatasetSource
    {
        _serviceCollection.AddSingleton<IDocumentsTrainDatasetSource, T>();
    }
    
    public void UseOptionsProvider<TProvider, TModel>(IConfiguration configuration) 
        where TProvider : class, INeuralOptionsProvider<TModel>
        where TModel : class, INeuralOptions
    {
        var section = configuration.GetSection(typeof(TModel).Name);
        _serviceCollection.AddSingleton<INeuralOptionsProvider<TModel>, TProvider>();
        _serviceCollection.Configure<TModel>(section);
    }
}