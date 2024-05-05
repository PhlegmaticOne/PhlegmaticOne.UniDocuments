using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Services.DocumentMapping;

namespace UniDocuments.Text.Root.Builders;

public class DocumentMapperInstallBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public DocumentMapperInstallBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public void UseInitializer<TDev, TProd>(bool useRealDatabase)
        where TDev : class, IDocumentMappingInitializer
        where TProd : class, IDocumentMappingInitializer
    {
        if (!useRealDatabase)
        {
            _serviceCollection.AddScoped<IDocumentMappingInitializer, TDev>();
        }
        else
        {
            _serviceCollection.AddScoped<IDocumentMappingInitializer, TProd>();
        }
    }
}