using Microsoft.Extensions.DependencyInjection;

namespace UniDocuments.Text.Features.Fingerprint.Services;

public static class FingerprintServiceInstaller
{
    public static IServiceCollection AddFingerprintService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IFingerprintsContainer, FingerprintsContainer>();
        serviceCollection.AddSingleton<IFingerprintComputer, FingerprintComputer>();
        serviceCollection.AddSingleton<IFingerprintSearcher, FingerprintSearcher>();
        return serviceCollection;
    }
}