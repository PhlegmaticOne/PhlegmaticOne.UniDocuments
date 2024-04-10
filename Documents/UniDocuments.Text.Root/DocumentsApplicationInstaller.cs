using Microsoft.Extensions.DependencyInjection;

namespace UniDocuments.Text.Root;

public static class DocumentsApplicationInstaller
{
    public static IServiceCollection AddDocumentsApplication(
        this IServiceCollection serviceCollection,
        Action<DocumentApplicationBuilder> builderAction)
    {
        var builder = new DocumentApplicationBuilder(serviceCollection);
        builderAction(builder);
        return serviceCollection;
    }
}