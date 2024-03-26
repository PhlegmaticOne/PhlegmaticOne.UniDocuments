using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Features.Text.Contracts;

namespace UniDocuments.Text.Features.Text.Services;

public static class TextServicesInstaller
{
    public static IServiceCollection AddDocumentTextLoader(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IDocumentTextLoader, DocumentTextLoader>();
        return serviceCollection;
    }
}