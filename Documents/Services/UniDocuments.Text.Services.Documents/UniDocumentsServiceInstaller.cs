using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Services.Documents;

namespace UniDocuments.Text.Services.Documents;

public static class UniDocumentsServiceInstaller
{
    public static IServiceCollection AddUniDocumentsService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IUniDocumentsCache, UniDocumentsCache>();
        serviceCollection.AddSingleton<IUniDocumentsService, UniDocumentsService>();
        return serviceCollection;
    }
}