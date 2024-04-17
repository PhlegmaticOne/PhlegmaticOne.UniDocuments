using Microsoft.Extensions.DependencyInjection;
using PhlegmaticOne.PasswordHasher.Implementation;

namespace PhlegmaticOne.PasswordHasher.Extensions;

public static class PasswordHasherExtensions
{
    public static IServiceCollection AddPasswordHasher(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IPasswordHasher, SecurePasswordHasher>();
        return serviceCollection;
    }
}