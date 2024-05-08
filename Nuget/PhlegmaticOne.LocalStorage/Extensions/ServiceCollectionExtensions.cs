using Microsoft.Extensions.DependencyInjection;
using PhlegmaticOne.LocalStorage.Implementation;

namespace PhlegmaticOne.LocalStorage.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStorage(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<ILocalStorageService, InMemoryLocalStorageService>();
    }
}