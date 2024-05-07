using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Providers.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Options;

namespace UniDocuments.Text.Root.Builders;

public class FingerprintingInstallBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public FingerprintingInstallBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }
       
    public void UseFingerprintsProvider<T>() where T : class, IFingerprintsProvider
    {
        _serviceCollection.AddScoped<IFingerprintsProvider, T>();
    }
    
    public void UseFingerprintContainer<T>() where T : class, IFingerprintContainer
    {
        _serviceCollection.AddSingleton<IFingerprintContainer, T>();
    }
    
    public void UseFingerprintHash<T>() where T : class, IFingerprintHash
    {
        _serviceCollection.AddSingleton<IFingerprintHash, T>();
    }
    
    public void UseFingerprintAlgorithm<T>() where T : class, IFingerprintAlgorithm
    {
        _serviceCollection.AddSingleton<IFingerprintAlgorithm, T>();
    }
    
    public void UseFingerprintsComparer<T>() where T : class, IFingerprintsComparer
    {
        _serviceCollection.AddSingleton<IFingerprintsComparer, T>();
    }
    
    public void UseOptionsProvider<T>(IConfiguration configuration) where T : class, IFingerprintOptionsProvider
    {
        _serviceCollection.AddSingleton<IFingerprintOptionsProvider, T>();
        _serviceCollection.Configure<FingerprintOptions>(configuration.GetSection(nameof(FingerprintOptions)));
    }
}