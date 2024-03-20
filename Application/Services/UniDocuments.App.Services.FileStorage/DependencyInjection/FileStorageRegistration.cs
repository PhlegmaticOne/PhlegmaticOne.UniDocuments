using Microsoft.Extensions.DependencyInjection;
using UniDocuments.App.Domain.Services.FileStorage;
using UniDocuments.App.Services.FileStorage.InMemory;
using UniDocuments.App.Services.FileStorage.Sql;

namespace UniDocuments.App.Services.FileStorage.DependencyInjection;

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