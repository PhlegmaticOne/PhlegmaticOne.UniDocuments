using Microsoft.Extensions.DependencyInjection;
using PhlegmaticOne.LocalStorage.Implementation;

namespace PhlegmaticOne.LocalStorage.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLocalStorage(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<ILocalStorageService, InMemoryLocalStorageService>();
    }

    public static IServiceCollection AddLocalStorage(this IServiceCollection serviceCollection,
        Action<ILocalStorageService> startConfigurationAction)
    {
        return serviceCollection.AddSingleton<ILocalStorageService>(x =>
        {
            var storage = new InMemoryLocalStorageService();
            startConfigurationAction(storage);
            return storage;
        });
    }
}