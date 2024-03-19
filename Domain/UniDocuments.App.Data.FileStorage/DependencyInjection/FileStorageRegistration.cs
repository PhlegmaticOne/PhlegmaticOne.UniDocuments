using Microsoft.Extensions.DependencyInjection;
using UniDocuments.App.Data.Files.InMemory;
using UniDocuments.App.Data.Files.Sql;
using UniDocuments.App.Domain.FileStorage;

namespace UniDocuments.App.Data.Files.DependencyInjection;

public static class FileStorageRegistration
{
    public static IServiceCollection AddFileStorage(this IServiceCollection serviceCollection, Func<bool> isUseInMemory)
    {
        if (isUseInMemory())
        {
            serviceCollection.AddSingleton<IFileStorage, FileStorageInMemory>();
        }
        else
        {
            serviceCollection.AddSingleton<IFileStorage, FileStorageSql>();
        }
        
        return serviceCollection;
    } 
}