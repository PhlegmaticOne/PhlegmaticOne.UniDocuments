using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Options;

namespace UniDocuments.Text.Root.Builders;

public class DocumentsNeuralInstallBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public DocumentsNeuralInstallBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public void UseDataSource<TDev, TProd>(bool isDevelopment) 
        where TDev : class, IDocumentsNeuralSource
        where TProd : class, IDocumentsNeuralSource
    {
        if (isDevelopment)
        {
            _serviceCollection.AddSingleton<IDocumentsNeuralSource, TDev>();
        }
        else
        {
            _serviceCollection.AddSingleton<IDocumentsNeuralSource, TProd>();
        }
    }
        
    public void UseDataHandler<T>() where T : class, IDocumentsNeuralDataHandler
    {
        _serviceCollection.AddSingleton<IDocumentsNeuralDataHandler, T>();
    }
    
    public void UseOptionsProvider<T>(IConfiguration configuration) where T : class, IDocumentNeuralOptionsProvider
    {
        _serviceCollection.AddSingleton<IDocumentNeuralOptionsProvider, T>();
        _serviceCollection.Configure<DocumentNeuralOptions>(configuration.GetSection(nameof(DocumentNeuralOptions)));
    }
}