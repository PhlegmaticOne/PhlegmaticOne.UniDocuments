using Microsoft.Extensions.DependencyInjection;
using UniDocuments.App.Domain.Services.FileStorage;
using UniDocuments.App.Services.FileStorage.InMemory;
using UniDocuments.App.Services.FileStorage.Sql;
using UniDocuments.App.Services.FileStorage.Sql.Connection;

namespace UniDocuments.App.Services.FileStorage.DependencyInjection;

public static class FileStorageRegistration
{
    public static IServiceCollection AddFileStorage(this IServiceCollection serviceCollection, Func<bool> isUseInMemory)
    {
        if (isUseInMemory())
        {
            serviceCollection.AddSingleton<FileStorageInMemory>();
            serviceCollection.AddSingleton<IFileStorage>(x => x.GetRequiredService<FileStorageInMemory>());
            serviceCollection.AddSingleton<IFileStorageIndexable>(x => x.GetRequiredService<FileStorageInMemory>());
            // serviceCollection.AddSingleton<IFileStorage, FileStorageInMemory>();
            // serviceCollection.AddSingleton<IFileStorageIndexable, FileStorageInMemory>();
        }
        else
        {
            serviceCollection.AddSingleton<IFileStorage, FileStorageSql>();
            serviceCollection.AddSingleton<ISqlConnectionProvider, SqlConnectionProvider>();
        }
        
        return serviceCollection;
    } 
}