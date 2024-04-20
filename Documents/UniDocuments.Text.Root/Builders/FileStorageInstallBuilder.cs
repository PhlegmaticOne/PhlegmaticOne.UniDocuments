using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Services.DocumentsStorage;

namespace UniDocuments.Text.Root.Builders;

public class FileStorageInstallBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public FileStorageInstallBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }
        
    public void UseSqlConnectionProvider<T>() where T : class, ISqlConnectionProvider
    {
        _serviceCollection.AddSingleton<ISqlConnectionProvider, T>();
    }
}