using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Services.Fingerprinting.Options;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;

namespace UniDocuments.Text.Root.Builders;

public class FingerprintingInstallBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public FingerprintingInstallBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }
        
    public void UseFingerprintContainer<T>() where T : class, IFingerprintContainer
    {
        _serviceCollection.AddSingleton<IFingerprintContainer, T>();
    }
    
    public void UseFingerprintContainerInitializer<TDev, TProd>(bool isDevelopment)
        where TDev : class, IFingerprintContainerInitializer
        where TProd : class, IFingerprintContainerInitializer
    {
        if (isDevelopment)
        {
            _serviceCollection.AddSingleton<IFingerprintContainerInitializer, TDev>();
        }
        else
        {
            _serviceCollection.AddSingleton<IFingerprintContainerInitializer, TProd>();
        }
    }
    
    public void UseFingerprintComputer<T>() where T : class, IFingerprintComputer
    {
        _serviceCollection.AddSingleton<IFingerprintComputer, T>();
    }
    
    public void UseFingerprintHash<T>() where T : class, IFingerprintHash
    {
        _serviceCollection.AddSingleton<IFingerprintHash, T>();
    }
    
    public void UseFingerprintSearcher<T>() where T : class, IFingerprintSearcher
    {
        _serviceCollection.AddSingleton<IFingerprintSearcher, T>();
    }
    
    public void UseFingerprintAlgorithm<T>() where T : class, IFingerprintAlgorithm
    {
        _serviceCollection.AddSingleton<IFingerprintAlgorithm, T>();
    }
    
    public void UseOptionsProvider<T>(IConfiguration configuration) where T : class, IFingerprintOptionsProvider
    {
        _serviceCollection.AddSingleton<IFingerprintOptionsProvider, T>();
        _serviceCollection.Configure<FingerprintOptions>(configuration.GetSection(nameof(FingerprintOptions)));
    }
}