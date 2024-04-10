using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Services.Neural;

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
}