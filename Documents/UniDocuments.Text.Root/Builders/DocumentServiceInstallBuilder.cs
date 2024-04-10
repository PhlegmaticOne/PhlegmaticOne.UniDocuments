using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Services.Documents;

namespace UniDocuments.Text.Root.Builders;

public class DocumentServiceInstallBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public DocumentServiceInstallBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }
        
    public void UseDocumentsCache<T>() where T : class, IUniDocumentsCache
    {
        _serviceCollection.AddSingleton<IUniDocumentsCache, T>();
    }
}