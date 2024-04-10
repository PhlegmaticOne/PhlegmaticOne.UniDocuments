using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Features.Text.Contracts;

namespace UniDocuments.Text.Root.Builders.Features;

public class TextFeatureInstallBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public TextFeatureInstallBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }
        
    public void UseTextLoader<T>() where T : class, IDocumentTextLoader
    {
        _serviceCollection.AddSingleton<IDocumentTextLoader, T>();
    }
}