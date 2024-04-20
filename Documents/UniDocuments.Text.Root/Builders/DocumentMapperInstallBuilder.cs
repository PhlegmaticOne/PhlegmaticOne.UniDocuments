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

    public void UseInitializer<TDev, TProd>(bool isDevelopment)
        where TDev : class, IDocumentMapperInitializer
        where TProd : class, IDocumentMapperInitializer
    {
        if (isDevelopment)
        {
            _serviceCollection.AddScoped<IDocumentMapperInitializer, TDev>();
        }
        else
        {
            _serviceCollection.AddScoped<IDocumentMapperInitializer, TProd>();
        }
    }
}